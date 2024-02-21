
using LukNotificator.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using TelegramBotHelper;
using TelegramBotHelper.Commands;
using TelegramBotHelper.Services;

namespace LukNotificator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();

            if (Environment.UserInteractive)
            {
                await StartInCons(host);
            }

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var builder = new ConfigurationBuilder()
                //.AddJsonFile($"appsettings.json", false, true)
                .AddJsonFile("secrets.json", false, true);

            var configuration = builder.Build();

            return Host.CreateDefaultBuilder(args)
                .UseWindowsService(options => { options.ServiceName = "YouTubeNotificator_Service"; })
                .ConfigureServices((_, services) =>
                {
                    services
                        .RegisterMain()
                        .AddSingleton<IConfiguration>(configuration)
                        .AddQuartz()
                        .AddMediatR(cfg =>
                        {
                            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                        });

                    if (!Environment.UserInteractive)
                    {
                        services.AddHostedService<Worker>();
                    }

                });
        }

        static async Task StartInCons(IHost host)
        {
            var taskManager = host.Services.GetRequiredService<INotificationTaskManager>();
            await taskManager.Start();

            var bot = host.Services.GetRequiredService<ITelegramBot>();
            bot.ReceiveMessage += async (sender, args) =>
            {
                var factory = host.Services.GetRequiredService<ITelegramCommandFactory>();
                var parser = host.Services.GetRequiredService<ITelegramCommandParser>();
                var cmdInfo = parser.Parse(args.Message);
                if (cmdInfo == null)
                {
                    return;
                }

                var ctx = new TelegramBotContext()
                {
                    TelegramChannelId = args.ChannelId
                };
                try
                {
                    var cmd = factory.Create(cmdInfo, ctx);
                    var mediator = host.Services.GetRequiredService<IMediator>();
                    await mediator.Send(cmd);
                }
                catch (Exception ex)
                {
                    var tServ = host.Services.GetRequiredService<ITelegramBot>();
                    await tServ.SendMessage(args.ChannelId, ex.Message);
                }

            };
        }

    }
}
