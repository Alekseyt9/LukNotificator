
using TelegramBotHelper.Services;

namespace LukNotificator.Test.Mock
{
    internal class TelegramBotMock : ITelegramBot
    {
        public event EventHandler<TelegramMessageEventArgs>? ReceiveMessage;

        public Task SendMessage(long channelId, string msg)
        {
            return Task.CompletedTask;
        }

    }
}
