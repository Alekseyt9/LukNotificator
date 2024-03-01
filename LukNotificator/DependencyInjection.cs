
using LukNotificator.Services;
using LukNotificator.Services.Exchange;
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
                .AddSingleton<IRepository, Repository>()
                .AddSingleton<IExchangeService, ExchangeService>()
                ;

            return services;
        }
    }
}
