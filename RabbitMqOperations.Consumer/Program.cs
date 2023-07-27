using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMqOperations.Common;
using RabbitMqOperations.Consumer;
using RabbitMqOperations.Consumer.Consumers;
using RabbitMqOperations.Core.Services.Url.Implementations;
using RabbitMqOperations.Core.Services.Url.Interfaces;

var builder = new HostBuilder()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true);
        config.AddEnvironmentVariables();

        if (args != null)
            config.AddCommandLine(args);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<RabbitMqConfig>(hostContext.Configuration.GetSection("AppConfig"));
        services.AddTransient<IUrlService, UrlService>();

        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.AddConsumer<UrlConsumer>(typeof(UrlConsumerDefinition));

            cfg.AddBus(context => RabbitMassTransitConfiguration.ConfigureBus(context));
        });

        services.AddHostedService<RabbitMqConsumerHostedService>();
    })
    .ConfigureLogging((hostingContext, logging) =>
    {
        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        logging.AddConsole();
    });

await builder.RunConsoleAsync();
