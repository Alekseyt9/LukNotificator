

using MediatR;
using TelegramBotHelper.Commands;

namespace LukNotificator.Commands
{
    internal class ListCommand : TelegramCommandBase, IRequest
    {
        public ListCommand(TelegramBotContext context) : base(context)
        {
        }
    }
}
