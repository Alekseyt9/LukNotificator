

using LukNotificator.Services;

namespace LukNotificator.Jobs
{
    internal class SellJobProcessor : ISellJobProcessor
    {
        public SellJobProcessor() { }

        public Task Execute()
        {
            return Task.CompletedTask;
        }

    }
}
