﻿using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RabbitMqOperations.Consumer
{
    public class RabbitMqConsumerHostedService : IHostedService
    {
        #region Field(s)
        private readonly IBusControl _bus;
        private readonly ILogger _logger;
        #endregion

        #region Constructor(s)
        public RabbitMqConsumerHostedService(IBusControl bus, ILoggerFactory loggerFactory)
        {
            _bus = bus;
            _logger = loggerFactory.CreateLogger<RabbitMqConsumerHostedService>();
        }
        #endregion

        #region Method(s)
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting bus");
            await _bus.StartAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping bus");
            return _bus.StopAsync(cancellationToken);
        }
        #endregion
    }
}
