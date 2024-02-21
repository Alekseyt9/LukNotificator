
using LukNotificator.Services;
using Microsoft.Extensions.DependencyInjection;
using TelegramBotHelper.Commands;

namespace TelegramBotHelper
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterMain(this IServiceCollection services)
        {
            services
                .AddTransient<ITelegramCommandFactory, TelegramCommandFactory>()
                .RegisterTelegramBotHelper()
                .AddTransient<INotificationTaskManager, NotificationTaskManager>()
                .AddTransient<IRepository, Repository>()
                .AddTransient<IExchangeService, ExchangeService>()
                ;

            return services;
        }
    }
}
