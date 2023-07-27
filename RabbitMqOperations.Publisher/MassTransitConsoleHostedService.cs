using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMqOperations.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqOperations.Publisher
{
    public class MassTransitConsoleHostedService :
        IHostedService
    {
        readonly IBusControl _bus;
        readonly ILogger _logger;

        public MassTransitConsoleHostedService(IBusControl bus, ILoggerFactory loggerFactory)
        {
            _bus = bus;
            _logger = loggerFactory.CreateLogger<MassTransitConsoleHostedService>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting bus");
            await _bus.StartAsync(cancellationToken)
                .ConfigureAwait(false);


            var i = 0;

            while (true)
            {
                i++;
                await _bus.Publish(new UrlMessage() { Url = "https://twitter.com/home" + i.ToString() }, cancellationToken)
                    .ConfigureAwait(false);

                await Task.Delay(1000);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping bus");
            return _bus.StopAsync(cancellationToken);
        }
    }
}
