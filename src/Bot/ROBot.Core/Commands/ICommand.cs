namespace ROBot.Core.Handlers
{
    public interface ICommand
    {
        ICommandSender Sender { get; }
        string Command { get; }
        string Arguments { get; }
    }

    public interface ICommandSender
    {
        string UserId { get; }
        string Username { get; }
        string DisplayName { get; }
        bool IsBroadcaster { get; }
        bool IsModerator { get; }
        bool IsSubscriber { get; }
        string ColorHex { get; }
    }
}