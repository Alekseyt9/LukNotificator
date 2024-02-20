
using System.Globalization;
using LukNotificator.Commands;
using TelegramBotHelper.Commands;

namespace LukNotificator.Services
{
    internal class TelegramCommandFactory : ITelegramCommandFactory
    {
        public TelegramCommandBase Create(CommandInfo cmdInfo, TelegramBotContext ctx)
        {
            switch (cmdInfo.Code)
            {
                case "start":
                    return new StartCommand(ctx);

                case "ls":
                    return new ListCommand(ctx);

                case "rm":
                    return new DelCommand(ctx)
                    {
                        Code = cmdInfo.Params.FirstOrDefault()
                    };

                case "add":
                    return new AddCommand(ctx)
                    {
                        Code = cmdInfo.Params.FirstOrDefault(),
                        Price = double.Parse(cmdInfo.Params.Skip(1).FirstOrDefault(), CultureInfo.InvariantCulture)
                    };

                default: throw new ArgumentException($"Wrong command {cmdInfo.Code}");
            }
        }

    }
}
