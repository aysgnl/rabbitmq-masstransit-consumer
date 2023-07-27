using MassTransit;
using RabbitMqOperations.Common.Messages;
using RabbitMqOperations.Core.Services.Url.Interfaces;

namespace RabbitMqOperations.Consumer.Consumers
{
    public class UrlConsumer : IConsumer<UrlMessage>
    {
        #region Field(s)
        private readonly IUrlService _urlService;
        #endregion

        #region Constructor(s)
        public UrlConsumer(IUrlService urlService)
        {
            _urlService = urlService;
        }
        #endregion

        #region Method(s)
        public async Task Consume(ConsumeContext<UrlMessage> context)
        {
            try
            {
                var url = context?.Message.Url;
                bool result = Uri.TryCreate(url, UriKind.Absolute, result: out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (!string.IsNullOrEmpty(url) && uriResult.IsAbsoluteUri)
                    await _urlService.LoggedUrl(url);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Consume Url Consumer", ex);
            }
        }
        #endregion
    }
}
