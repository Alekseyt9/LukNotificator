
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
                ;

            return services;
        }
    }
}
