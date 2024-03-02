namespace LukNotificator.Services.Exchange
{
    internal interface IExchangeService
    {
        Task<IDictionary<string, double>> GetUsdtPairs(string[] codes);

        Task<IEnumerable<ExCurInfo>> GetOwnCurrencies();

        Task<bool> Sell(string code, double value);

        Task<bool> Buy(string code, double value);

    }
}
