
using System.Text;
using LukNotificator.Entity;
using LukNotificator.Services;
using LukNotificator.Services.Exchange;
using MediatR;
using TelegramBotHelper.Services;

namespace LukNotificator.Commands.Handlers
{
    internal class ListCommandHandler(
            IUserRepository userRep, ICurrencyRepository curRep, ITelegramBot telegramBot, IExchangeService exService) 
        : IRequestHandler<ListCommand>
    {
        public class Sort
        {
            public double Proc { get; set; }
            public Currency Currency { get; set; }
        }

        public async Task Handle(ListCommand request, CancellationToken cancellationToken)
        {
            var user = await userRep.Get(request.Context.TelegramChannelId);
            var curs = await curRep.GetAll(user);
            var sb = new StringBuilder();

            var curArr = curs.Select(c => c.Code).ToArray();
            var pairs = await exService.GetUsdtPairs(curArr);
            var sorted = curs.Select(x => new Sort() { Currency = x, Proc = (pairs[x.Code] - x.Price) / x.Price }).OrderByDescending(x => x.Proc);

            foreach (var s in sorted)
            {
                var cur = s.Currency;
                sb.AppendLine($"{cur.Code}: {cur.Price} => {pairs[cur.Code]} ({(s.Proc * 100).ToString("f3")}%)");
            }

            await telegramBot.SendMessage(request.Context.TelegramChannelId, sb.ToString());
        }

    }
}
