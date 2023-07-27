using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RabbitMqOperations.Common
{
    public class RabbitMassTransitConfiguration
    {
        public static IBusControl ConfigureBus(IBusRegistrationContext context)
        {
            var appConfig = context.GetRequiredService<IOptions<RabbitMqConfig>>().Value;

            if (appConfig is null)
            {
                throw new ArgumentNullException(nameof(appConfig));
            }

            return ConfigureUsingRabbitMq(context, appConfig);
        }

        public static IBusControl ConfigureUsingRabbitMq(IBusRegistrationContext context, RabbitMqConfig rabbitMQConfig)
        {
            if (rabbitMQConfig is null)
            {
                throw new ArgumentNullException(nameof(rabbitMQConfig));
            }

            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(rabbitMQConfig.Host, rabbitMQConfig.VirtualHost, h =>
                {
                    h.Username(rabbitMQConfig.UserName);
                    h.Password(rabbitMQConfig.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        }
    }
}