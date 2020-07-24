namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerNpcActionRequest
    {
        public const short OpCode = 31;
        public int NpcServerId { get; set; }
        public int ActionId { get; set; }
        public int ParameterId { get; set; }
    }
}
