
using MediatR;
using TelegramBotHelper.Commands;

namespace LukNotificator.Commands
{
    internal class StartCommand : TelegramCommandBase, IRequest
    {
        public StartCommand(TelegramBotContext context) : base(context)
        {
        }


    }
}
