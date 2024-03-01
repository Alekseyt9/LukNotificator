namespace LukNotificator.Services.Exchange
{
    internal interface IExchangeService
    {
        Task<IDictionary<string, double>> GetUsdtPairs(string[] codes);

        Task<IEnumerable<ExCurInfo>> GetOwnCurrencies();

        Task Sell(string code, double value);

    }
}
