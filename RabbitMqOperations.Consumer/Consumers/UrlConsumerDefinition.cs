using MassTransit;

namespace RabbitMqOperations.Consumer.Consumers
{
    public class UrlConsumerDefinition : ConsumerDefinition<UrlConsumer>
    {
        public UrlConsumerDefinition()
        {
            // override the default endpoint name
            EndpointName = "urls";

            // limit the number of messages consumed concurrently
            // this applies to the consumer only, not the endpoint
            ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<UrlConsumer> consumerConfigurator)
        {
            // configure message retry with millisecond intervals
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

            // use the outbox to prevent duplicate events from being published
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
