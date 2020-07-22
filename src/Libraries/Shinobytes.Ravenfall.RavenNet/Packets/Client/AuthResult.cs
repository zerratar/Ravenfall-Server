namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public enum AuthResult
    {
        Success = 0,
        InvalidPassword = 1,
        TemporaryDisabled = 2,
        PermanentlyDisabled = 3
    }
}
