
namespace TelegramBotHelper.Commands
{
    public interface ITelegramCommandFactory
    {
        TelegramCommandBase Create(CommandInfo cmdInfo, TelegramBotContext ctx);
    }
}
