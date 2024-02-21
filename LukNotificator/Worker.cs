
using LukNotificator.Services;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TelegramBotHelper.Commands;
using TelegramBotHelper.Services;

namespace LukNotificator
{
    internal class Worker(INotificationTaskManager taskManager, ITelegramBot bot,
            ITelegramCommandFactory factory, ITelegramCommandParser parser,
            IMediator mediator, ILogger<Worker> logger)
        : BackgroundService
    {
        private readonly INotificationTaskManager _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
        private readonly ITelegramBot _bot = bot ?? throw new ArgumentNullException();
        private readonly ITelegramCommandFactory _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        private readonly ITelegramCommandParser _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private readonly ILogger<Worker> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[!] Service running");
            try
            {
                await Start();
                _logger.LogInformation("[!] Start()");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[!] " + ex.Message);
            }

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[!] Service stopped");

            await base.StopAsync(cancellationToken);
        }

        private async Task Start()
        {
            await _taskManager.Start();

            _bot.ReceiveMessage += async (sender, args) =>
            {
                var cmdInfo = _parser.Parse(args.Message);
                if (cmdInfo == null)
                {
                    return;
                }

                var ctx = new TelegramBotContext()
                {
                    TelegramChannelId = args.ChannelId
                };
                var cmd = (TelegramCommandBase)_factory.Create(cmdInfo, ctx);

                await _mediator.Send(cmd);
            };
        }

    }
}
