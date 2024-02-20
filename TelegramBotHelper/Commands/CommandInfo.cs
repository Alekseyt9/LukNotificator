

namespace TelegramBotHelper.Commands
{
    public class CommandInfo
    {
        public string Code { get; set; }

        public ICollection<string> Params { get; set; } = new List<string>();
    }
}
