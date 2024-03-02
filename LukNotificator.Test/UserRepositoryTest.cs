
using LukNotificator.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LukNotificator.Test
{
    public class UserRepositoryTest
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRep;

        public UserRepositoryTest()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("secrets.json", false, true);

            _configuration = builder.Build();
            _configuration["pgconnstr"] = $"Server=37.77.105.224; Port=5432; Database=lukbot; User Id=alex; Password={_configuration["pgpass"]};";
            _userRep = new UserRepository(_configuration);
        }

        [Fact]
        public async Task Test1()
        {
            var users = await _userRep.GetAll();
        }

        [Fact]
        public async Task Test2()
        {
            var user = await _userRep.GetOrCreate(1234);
            var users = await _userRep.GetAll();
        }

    }
}
