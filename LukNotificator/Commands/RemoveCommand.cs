
using MediatR;
using TelegramBotHelper.Commands;

namespace LukNotificator.Commands
{
    internal class RemoveCommand : TelegramCommandBase, IRequest
    {
        public RemoveCommand(TelegramBotContext context) : base(context)
        {

        }

        public string Code { get; set; }

    }
}
