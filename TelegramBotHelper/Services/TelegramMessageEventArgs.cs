

namespace TelegramBotHelper.Services
{
    public class TelegramMessageEventArgs : EventArgs
    {
        public long ChannelId { get; set; }
        public string Message { get; set; }
    }
}
