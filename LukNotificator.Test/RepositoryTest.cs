
using LukNotificator.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LukNotificator.Test
{
    public class RepositoryTest
    {
        readonly IConfiguration _configuration;
        readonly IRepository _repository;

        public RepositoryTest()
        {
            var builder = new ConfigurationBuilder()
                //.AddJsonFile($"appsettings.json", false, true);
                .AddJsonFile("secrets.json", false, true);

            _configuration = builder.Build();
            _configuration["pgconnstr"] = $"Server=37.77.105.224; Port=5432; Database=lukbot; User Id=alex; Password={_configuration["pgpass"]};";
            _repository = new Repository(_configuration);
        }

        [Fact]
        public async Task Test1()
        {
            var users = await _repository.GetUsers();
        }

        [Fact]
        public async Task Test2()
        {
            var user = await _repository.GetOrCreateUser(1234);
            var users = await _repository.GetUsers();
        }

    }
}
