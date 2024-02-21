
using System.Text;
using LukNotificator.Services;
using MediatR;
using TelegramBotHelper.Services;

namespace LukNotificator.Commands.Handlers
{
    internal class ListCommandHandler(IRepository repository, ITelegramBot telegramBot, IExchangeService exService) : IRequestHandler<ListCommand>
    {
        public async Task Handle(ListCommand request, CancellationToken cancellationToken)
        {
            var user = await repository.GetUser(request.Context.TelegramChannelId);
            var curs = await repository.GetCurrencies(user);
            var sb = new StringBuilder();

            var curArr = curs.Select(c => c.Code).ToArray();
            var pairs = await exService.GetUsdtPairs(curArr);

            foreach (var cur in curs)
            {
                var p = (pairs[cur.Code] - cur.Price) / cur.Price;
                sb.AppendLine($"{cur.Code}: {cur.Price} => {pairs[cur.Code]} ({(p * 100).ToString("f3")}%)");
            }

            await telegramBot.SendMessage(request.Context.TelegramChannelId, sb.ToString());
        }

    }
}
