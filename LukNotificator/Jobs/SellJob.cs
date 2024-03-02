
using LukNotificator.Services.Exchange;
using LukNotificator.Services;
using Quartz;
using TelegramBotHelper.Services;

namespace LukNotificator.Jobs
{
    internal class SellJob(IUserRepository userRep, IExchangeService exchangeService, ITelegramBot telegramBot, ISellJobProcessor sellJobProcessor) 
        : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await sellJobProcessor.Execute();
        }

    }
}
