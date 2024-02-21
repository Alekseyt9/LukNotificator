

namespace LukNotificator.Services
{
    internal interface IExchangeService
    {
        Task<IDictionary<string, double>> GetUsdtPairs(string[] codes);

    }
}
