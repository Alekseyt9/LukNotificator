﻿
using System.Text;
using LukNotificator.Services;
using Quartz;
using TelegramBotHelper.Services;

namespace LukNotificator.Jobs
{
    internal class NotificationJob(IRepository repository, IExchangeService exchangeService, ITelegramBot telegramBot)
        : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var curs = await repository.GetCurrencies();
            var filtCurs = curs.DistinctBy(x => x.Code);
            var priceDict = await exchangeService.GetUsdtPairs(filtCurs.Select(x => x.Code).ToArray());
            var userMap = (await repository.GetUsers()).ToDictionary(x => x.Id, y => y.ChannelId);

            var uGroups = curs.GroupBy(x => x.UserId);
            foreach (var group in uGroups)
            {
                var sb = new StringBuilder();
                foreach (var cur in group)
                {
                    var p = (cur.Price - priceDict[cur.Code]) / cur.Price;
                    if (p > 0.03)
                    {
                        sb.AppendLine($"{cur.Code}: {cur.Price} => {priceDict[cur.Code]} raised on 3% ");
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