using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogAdminUiIntegrationTest.Data.Service
{
    public class MessagePublishService
    {
        private static readonly IConfiguration Configuration = BrowserUtility.InitConfiguration();

        public async Task<DeliveryResult<Null, string>> PublishMessage(string topic, string json)
        {

            var config = new ProducerConfig
            {
                BootstrapServers = Configuration.GetValue<string>("BootstrapServer"),
                Acks = Acks.Leader,
                BrokerVersionFallback = "0.10.0.0",
                ApiVersionFallbackMs = 0,
                SocketKeepaliveEnable = true,
                SecurityProtocol = (SecurityProtocol)Enum.Parse(typeof(SecurityProtocol), "Plaintext"),
                SslCertificatePem = null,
                SslKeyPem = null,
                SslKeyPassword = null,
                EnableSslCertificateVerification = false
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var message = new Message<Null, string>
                {
                    Value = json
                };
                var header = new Header("Authorization", Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JwtToken")));

                message.Headers = new Headers
                    {
                        header
                    };

               return await producer.ProduceAsync(topic, message);
            };
        }
    }
}
