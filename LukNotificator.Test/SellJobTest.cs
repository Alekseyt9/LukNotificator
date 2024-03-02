
using LukNotificator.Jobs;
using LukNotificator.Services;

using LukNotificator.Test.Mock;
using Microsoft.Extensions.Configuration;

namespace LukNotificator.Test
{
    public class SellJobTest
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRep;

        public SellJobTest()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("secrets.json", false, true);

            _configuration = builder.Build();
            _configuration["pgconnstr"] = $"Server=37.77.105.224; Port=5432; Database=lukbot; User Id=alex; Password={_configuration["pgpass"]};";
            _userRep = new UserRepository(_configuration);
        }

        public Task Test1()
        {
            var telServ = new TelegramBotMock();
            var exServ = new ExServiceMock();
            var jobProc = new SellJobProcessor();

            var range = new[] { 1.0, 1.1, 1.9, 1.7, 2.0, 1.6 };
            foreach (var val in range)
            {
                exServ.SetTest("XRP", val);
                jobProc.Execute();
            }

            return Task.CompletedTask;
        }

    }
}
