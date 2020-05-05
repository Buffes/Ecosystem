namespace Ecosystem.Console
{
    public interface ICommandSender
    {
        void SendMessage(string message);
        void SendMessage(string message, MessageType messageType);
    }
}
