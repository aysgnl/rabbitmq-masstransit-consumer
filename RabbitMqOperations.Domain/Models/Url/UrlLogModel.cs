using System.Net;

namespace RabbitMqOperations.Domain.Models.Url
{
    public class UrlLogModel
    {
        public string ServiceName { get; set; }
        public string Url { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
