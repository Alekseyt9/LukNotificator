
using TelegramBotHelper.Commands;

namespace LukNotificator.Commands
{
    internal class DelCommand : TelegramCommandBase
    {
        public DelCommand(TelegramBotContext context) : base(context)
        {

        }

        public string Code { get; set; }

    }
}
