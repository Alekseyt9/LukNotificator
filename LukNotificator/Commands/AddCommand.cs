
using TelegramBotHelper.Commands;

namespace LukNotificator.Commands
{
    internal class AddCommand : TelegramCommandBase
    {
        public AddCommand(TelegramBotContext context) : base(context)
        {
        }

        public string Code { get; set; }

        public double Price { get; set; }

    }
}
