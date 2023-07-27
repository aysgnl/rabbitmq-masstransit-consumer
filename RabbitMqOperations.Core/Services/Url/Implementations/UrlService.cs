using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMqOperations.Core.Services.Url.Interfaces;
using RabbitMqOperations.Domain.Models.Url;

namespace RabbitMqOperations.Core.Services.Url.Implementations
{
    public class UrlService : IUrlService
    {
        #region Field(s)
        private ILogger<UrlService> _logger;
        #endregion

        #region Constructor(s)
        public UrlService(ILogger<UrlService> logger)
        {
            _logger = logger;
        }
        #endregion

        #region Method(s)
        public async Task LoggedUrl(string url)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

                var loggedMessage = new UrlLogModel()
                {
                    ServiceName = "RabbitListener",
                    Url = url,
                    StatusCode = response.StatusCode
                };

                _logger.LogInformation(message: JsonConvert.SerializeObject(loggedMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError(message: $"UrlService LoggedUrl was error. Exception: {ex}");
            }
        }
        #endregion
    }
}
