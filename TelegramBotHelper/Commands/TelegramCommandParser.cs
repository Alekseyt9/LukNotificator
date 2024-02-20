

namespace TelegramBotHelper.Commands
{
    internal class TelegramCommandParser : ITelegramCommandParser
    {
        public CommandInfo Parse(string message)
        {
            var res = new CommandInfo();
            var arr = message.Split(" ")
                .Where(x => !string.IsNullOrEmpty(x.Trim(' '))).ToArray();

            string cmd;
            if (arr.Length > 0)
            {
                cmd = arr[0];
                res.Code = cmd.TrimStart('/');
            }
            else
            {
                throw new ArgumentException("Cmd missed");
            }

            if (arr.Length > 1)
            {
                for (var i = 1; i < arr.Length; i++)
                {
                    res.Params.Add(arr[i]);
                }
            }

            return res;
        }


    }
}
