
namespace TelegramBotHelper.Commands
{
    public class TelegramCommandBase
    {
        protected TelegramCommandBase(TelegramBotContext context)
        {
            Context = context;
        }

        public TelegramBotContext Context { get; }
    }
}
