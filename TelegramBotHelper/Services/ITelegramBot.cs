

namespace TelegramBotHelper.Services
{
    public interface ITelegramBot
    {
        event EventHandler<TelegramMessageEventArgs> ReceiveMessage;

        Task SendMessage(long channelId, string msg);
    }
}
