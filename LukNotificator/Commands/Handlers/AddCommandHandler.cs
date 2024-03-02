
using LukNotificator.Services;
using MediatR;
using TelegramBotHelper.Services;

namespace LukNotificator.Commands.Handlers
{
    internal class AddCommandHandler(IUserRepository userRep, ICurrencyRepository curRep, ITelegramBot telegramBot) 
        : IRequestHandler<AddCommand>
    {
        public async Task Handle(AddCommand request, CancellationToken cancellationToken)
        {
            var user = await userRep.GetOrCreate(request.Context.TelegramChannelId);
            await curRep.Add(user, request.Code, request.Price);
            await telegramBot.SendMessage(request.Context.TelegramChannelId,
                $"currency added {request.Code} {request.Price}");
        }

    }
}
