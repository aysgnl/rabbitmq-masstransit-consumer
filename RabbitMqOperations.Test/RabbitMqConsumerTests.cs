using MassTransit;
using MassTransit.Testing;
using Moq;
using RabbitMqOperations.Common.Messages;
using RabbitMqOperations.Consumer.Consumers;
using RabbitMqOperations.Core.Services.Url.Interfaces;

namespace RabbitMqOperations.Test
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public async Task UrlConsumer_Should_Log_Valid_Url()
        {
            var validUrl = "https://www.akakce.com/";
            var mockUrlService = new Mock<IUrlService>();
            var consumer = new UrlConsumer(mockUrlService.Object);

            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => consumer);

            var urlMessage = new UrlMessage { Url = validUrl };

            
            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send(urlMessage);
                               
                Assert.IsTrue(await harness.Consumed.Any<UrlMessage>());
            }
            finally
            {
                await harness.Stop();
            }
                       
            mockUrlService.Verify(service => service.LoggedUrl(validUrl), Times.Once);
        }

        [Test]
        public async Task UrlConsumer_Should_Not_Log_Invalid_Url()
        {
            var invalidUrl = "not_a_valid_url";
            var mockUrlService = new Mock<IUrlService>();
            var consumer = new UrlConsumer(mockUrlService.Object);

            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => consumer);

            var urlMessage = new UrlMessage { Url = invalidUrl };

           
            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send(urlMessage);
              
                Assert.IsTrue(await harness.Consumed.Any<UrlMessage>());
            }
            finally
            {
                await harness.Stop();
            }
            
            mockUrlService.Verify(service => service.LoggedUrl(invalidUrl), Times.Never);
        }

        [Test]
        public async Task TestMyConsumer()
        {
            var message = new UrlMessage { Url = "https://www.akakce.com/" };

            var mockUrlService = new Mock<IUrlService>();
           
            var consumer = new UrlConsumer(mockUrlService.Object);

            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => consumer);

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send(message);

                mockUrlService.VerifyNoOtherCalls();
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}


