using System;
using System.Linq;
using ROBot.Core.Repositories;

namespace ROBot.Core.Twitch
{
    public class TwitchUserStore : FileBasedRepository<TwitchUser>, ITwitchUserStore
    {
        public TwitchUserStore()
            : base("E:\\stream\\twitch-users.json")
        {
        }

        public ITwitchUser Get(string username)
        {
            lock (Mutex)
            {
                var user = this.Items.FirstOrDefault(x => x.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (user == null)
                {
                    user = new TwitchUser(username, null, 1000);
                    this.Store(user);
                }
                return new StoreBoundTwitchUser(this, user);
            }
        }

        private class StoreBoundTwitchUser : ITwitchUser
        {
            private readonly TwitchUserStore store;
            private readonly ITwitchUser user;

            public StoreBoundTwitchUser(
                TwitchUserStore store,
                ITwitchUser user)
            {
                this.store = store;
                this.user = user;
            }

            public string Name => this.user.Name;
            public string Alias => this.user.Alias;
            public long Credits => this.user.Credits;
            public bool CanAfford(long cost) => this.user.CanAfford(cost);

            public void RemoveCredits(long amount)
            {
                this.user.RemoveCredits(amount);
                this.store.Save();
            }

            public void AddCredits(long amount)
            {
                this.user.AddCredits(amount);
                this.store.Save();
            }

            public bool CanUseCommand(string command)
            {
                return this.user.CanUseCommand(command);
            }

            public void UseCommand(string command, TimeSpan cooldown)
            {
                this.user.UseCommand(command, cooldown);
            }

            public TimeSpan GetCooldown(string command)
            {
                return this.user.GetCooldown(command);
            }
        }
    }
}