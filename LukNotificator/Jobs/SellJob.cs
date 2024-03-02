
using LukNotificator.Services.Exchange;
using LukNotificator.Services;
using Quartz;
using TelegramBotHelper.Services;

namespace LukNotificator.Jobs
{
    internal class SellJob(IUserRepository userRep, IExchangeService exchangeService, ITelegramBot telegramBot) 
        : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var curList = await exchangeService.GetOwnCurrencies();

        }

    }
}
