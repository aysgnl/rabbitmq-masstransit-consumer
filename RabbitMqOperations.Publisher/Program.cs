using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMqOperations.Common;
using RabbitMqOperations.Publisher;

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

                    services.AddMassTransit(cfg =>
                    {
                        cfg.SetKebabCaseEndpointNameFormatter();

                        cfg.AddBus(context => RabbitMassTransitConfiguration.ConfigureBus(context));
                    });

                    services.AddHostedService<RabbitMqPublisherHostedService>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

await builder.RunConsoleAsync();