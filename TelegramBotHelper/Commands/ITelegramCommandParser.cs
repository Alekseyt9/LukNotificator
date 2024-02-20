

namespace TelegramBotHelper.Commands
{
    public interface ITelegramCommandParser
    {
        CommandInfo Parse(string message);
    }
}
