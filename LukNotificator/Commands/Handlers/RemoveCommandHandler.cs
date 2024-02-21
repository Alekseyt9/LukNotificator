
using LukNotificator.Services;
using MediatR;
using TelegramBotHelper.Services;

namespace LukNotificator.Commands.Handlers
{
    internal class RemoveCommandHandler(IRepository repository, ITelegramBot telegramBot) : IRequestHandler<RemoveCommand>
    {
        public async Task Handle(RemoveCommand request, CancellationToken cancellationToken)
        {
            var user = await repository.GetOrCreateUser(request.Context.TelegramChannelId);
            await repository.RemoveCurrency(user, request.Code);
            await telegramBot.SendMessage(request.Context.TelegramChannelId,
                $"currency removed {request.Code}");
        }

    }
}
