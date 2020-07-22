
namespace ROBot.Core.Twitch
{
    public class TwitchChatMessagePart
    {
        public TwitchChatMessagePart(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; }
        public string Value { get; }
    }

    public class TwitchUserLeft
    {
        public string Name { get; }

        public TwitchUserLeft(string name)
        {
            Name = name;
        }
    }

    public class TwitchUserJoined
    {
        public string Name { get; }

        public TwitchUserJoined(string name)
        {
            Name = name;
        }
    }

    public class TwitchCheer
    {
        public string UserId { get; }
        public string UserName { get; }
        public string DisplayName { get; }
        public int Bits { get; }

        public TwitchCheer(
            string userId,
            string userName,
            string displayName,
            int bits)
        {
            UserId = userId;
            UserName = userName;
            DisplayName = displayName;
            Bits = bits;
        }

    }

    public class TwitchSubscription
    {
        public string UserId { get; }
        public string ReceiverUserId { get; }
        public string UserName { get; }
        public string DisplayName { get; }
        public int Months { get; }
        public bool IsNew { get; }

        public TwitchSubscription(
            string userId,
            string userName,
            string displayName,
            string receiverUserId,
            int months,
            bool isNew)
        {
            UserId = userId;
            ReceiverUserId = receiverUserId;
            UserName = userName;
            DisplayName = displayName;
            Months = months;
            IsNew = isNew;
        }
    }

}