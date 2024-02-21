
using Microsoft.Extensions.DependencyInjection;
using TelegramBotHelper.Commands;
using TelegramBotHelper.Services;

namespace TelegramBotHelper
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterTelegramBotHelper(this IServiceCollection services)
        {
            services
                .AddTransient<ITelegramCommandParser, TelegramCommandParser>()
                .AddSingleton<ITelegramBot, TelegramBot>();
            return services;
        }
    }
}
