
using System.Text;
using LukNotificator.Services;
using LukNotificator.Services.Exchange;
using Quartz;
using TelegramBotHelper.Services;

namespace LukNotificator.Jobs
{
    internal class NotificationJob(
            ICurrencyRepository curRep, IUserRepository userRep, IExchangeService exchangeService, ITelegramBot telegramBot)
        : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var curs = await curRep.GetCurrencies();
            var filtCurs = curs.DistinctBy(x => x.Code);
            var priceDict = await exchangeService.GetUsdtPairs(filtCurs.Select(x => x.Code).ToArray());
            var userMap = (await userRep.GetUsers()).ToDictionary(x => x.Id, y => y.ChannelId);

            var uGroups = curs.Where(x => !x.IsTriggered).GroupBy(x => x.UserId);
            foreach (var group in uGroups)
            {
                var sb = new StringBuilder();
                foreach (var cur in group)
                {
                    var p = (priceDict[cur.Code] - cur.Price) / cur.Price;
                    if (p > 0.03)
                    {
                        sb.AppendLine($"signal {cur.Code}: {cur.Price} => {priceDict[cur.Code]} raised on {(p * 100).ToString("f3")}% ");
                        await curRep.UpdateCurrency(cur.Id, true);
                    }
                }

                if (sb.Length > 0)
                {
                    var chanId = userMap[group.Key];
                    await telegramBot.SendMessage(chanId, sb.ToString());
                }
            }

        }

    }
}
