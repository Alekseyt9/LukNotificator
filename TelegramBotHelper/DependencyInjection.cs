
using Microsoft.Extensions.DependencyInjection;
using TelegramBotHelper.Commands;

namespace TelegramBotHelper
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterTelegramBotHelper(this IServiceCollection services)
        {
            services
                .AddTransient<ITelegramCommandParser, TelegramCommandParser>();
            return services;
        }
    }
}
