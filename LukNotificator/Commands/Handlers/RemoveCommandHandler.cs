
using LukNotificator.Services;
using MediatR;
using TelegramBotHelper.Services;

namespace LukNotificator.Commands.Handlers
{
    internal class RemoveCommandHandler(IUserRepository userRep, ICurrencyRepository curRep, ITelegramBot telegramBot) 
        : IRequestHandler<RemoveCommand>
    {
        public async Task Handle(RemoveCommand request, CancellationToken cancellationToken)
        {
            var user = await userRep.GetOrCreateUser(request.Context.TelegramChannelId);
            await curRep.RemoveCurrency(user, request.Code);
            await telegramBot.SendMessage(request.Context.TelegramChannelId,
                $"currency removed {request.Code}");
        }

    }
}
