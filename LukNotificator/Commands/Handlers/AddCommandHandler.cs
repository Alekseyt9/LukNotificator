
using LukNotificator.Services;
using MediatR;
using TelegramBotHelper.Services;

namespace LukNotificator.Commands.Handlers
{
    internal class AddCommandHandler(IRepository repository, ITelegramBot telegramBot) : IRequestHandler<AddCommand>
    {
        public async Task Handle(AddCommand request, CancellationToken cancellationToken)
        {
            var user = await repository.GetOrCreateUser(request.Context.TelegramChannelId);
            await repository.AddCurrency(user, request.Code, request.Price);
            await telegramBot.SendMessage(request.Context.TelegramChannelId,
                $"currency added {request.Code} {request.Price}");
        }

    }
}
