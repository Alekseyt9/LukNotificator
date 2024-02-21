
using MediatR;

namespace LukNotificator.Commands.Handlers
{
    internal class AddCommandHandler : IRequestHandler<AddCommand>
    {
        public Task Handle(AddCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
