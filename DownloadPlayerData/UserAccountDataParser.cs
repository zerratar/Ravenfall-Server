using System;
using System.Collections.Generic;

namespace DownloadPlayerData
{
    public class UserAccountDataParser
    {
        public static IReadOnlyList<UserAccountData> Parse(string file)
        {
            var data = System.IO.File.ReadAllLines(file);
            var list = new List<UserAccountData>();
            for (var i = 1; i < data.Length; ++i)
            {
                var line = data[i];
                var details = line.Split('\t');
                list.Add(new UserAccountData
                {
                    Id = Guid.Parse(details[0]),
                    UserId = details[1],
                    UserName = details[2],
                    DisplayName = details[3],
                    Email = details[4],
                    PasswordHash = details[5],
                    // IsAdmin = details[6],
                    // IsModerator = details[7],
                    Created = DateTime.Parse(details[8])
                });
            }
            return list;
        }
    }
}
