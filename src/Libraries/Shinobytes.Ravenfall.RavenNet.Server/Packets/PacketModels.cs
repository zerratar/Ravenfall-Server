
using System;

namespace Shinobytes.Ravenfall.RavenNet.Server.Packets
{
    public class ServerHello
    {
        public const short OpCode = 1000;
        public string Name { get; set; }
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
    }

    public class AuthChallenge
    {
        public const short OpCode = 1001;
        public Guid CorrelationId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientVersion { get; set; }
    }

    public class ServerStats
    {
        public const short OpCode = 1002;
        public double CpuUsage { get; set; }
        public double MemUsage { get; set; }
        public int PlayerCount { get; set; }
        public int NpcCount { get; set; }
    }

    public class AuthChallengeResponse
    {

        public const short OpCode = 2001;
        public Guid CorrelationId { get; set; }
        public int Status { get; set; }
        public byte[] SessionKeys { get; set; }
    }

    public class Dummy
    {
        public const short OpCode = 1999;
        public int Test { get; set; }
    }
}
