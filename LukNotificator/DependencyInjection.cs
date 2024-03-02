
using LukNotificator.Services;
using LukNotificator.Services.Exchange;
using LukNotificator.Services.Repository;
using Microsoft.Extensions.DependencyInjection;
using TelegramBotHelper.Commands;

namespace TelegramBotHelper
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterMain(this IServiceCollection services)
        {
            services
                .AddSingleton<ITelegramCommandFactory, TelegramCommandFactory>()
                .RegisterTelegramBotHelper()
                .AddSingleton<INotificationTaskManager, NotificationTaskManager>()
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<ICurrencyRepository, CurrencyRepository>()
                .AddSingleton<IExchangeService, ExchangeService>()
                .AddSingleton<IOwnCurRepository, OwnCurRepository>()
                ;

            return services;
        }
    }
}
