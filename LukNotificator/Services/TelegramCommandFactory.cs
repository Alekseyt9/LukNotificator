
using System.Globalization;
using LukNotificator.Commands;
using TelegramBotHelper.Commands;
using static System.Net.Mime.MediaTypeNames;

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
                    if (cmdInfo.Params.Count != 1)
                    {
                        throw new ArgumentException($"Error. Command {cmdInfo.Code} must contain 1 parameters");
                    }
                    return new RemoveCommand(ctx)
                    {
                        Code = cmdInfo.Params.FirstOrDefault()
                    };

                case "add":
                    if (cmdInfo.Params.Count != 2)
                    {
                        throw new ArgumentException($"Error. Command {cmdInfo.Code} must contain 2 parameters");
                    }

                    var priceStr = cmdInfo.Params.Skip(1).FirstOrDefault().TrimStart('$').Replace(',', '.'); ;
                    return new AddCommand(ctx)
                    {
                        Code = cmdInfo.Params.FirstOrDefault(),
                        Price = double.Parse(priceStr, CultureInfo.InvariantCulture)
                    };

                default: throw new ArgumentException($"Wrong command {cmdInfo.Code}");
            }
        }

    }
}
