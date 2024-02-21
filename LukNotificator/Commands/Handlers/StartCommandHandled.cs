
using MediatR;

namespace LukNotificator.Commands.Handlers
{
    internal class StartCommandHandled : IRequestHandler<StartCommand>
    {
        public Task Handle(StartCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
