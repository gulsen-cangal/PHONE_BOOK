using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Report.API.Constants;
using Newtonsoft.Json;
using System.Text;
using Report.API.Data;
using Report.API.Services.Interfaces;

namespace Report.API.ServiceExtension
{
    public static class RabbitMQConsumer
    {
        public static IApplicationBuilder UseRabbitMq(this IApplicationBuilder app)
        {
            var reportSettings = app.ApplicationServices.GetRequiredService<IOptions<ReportSettings>>().Value;

            var conn = reportSettings.RabbitMqConsumer;

            var createDocumentQueue = "create_document_queue";
            var documentCreateExchange = "document_create_exchange";

            ConnectionFactory connectionFactory = new()
            {
                Uri = new Uri(conn)
            };

            var connection = connectionFactory.CreateConnection();

            var channel = connection.CreateModel();
            channel.ExchangeDeclare(documentCreateExchange, "direct");

            channel.QueueDeclare(createDocumentQueue, false, false, false);
            channel.QueueBind(createDocumentQueue, documentCreateExchange, createDocumentQueue);

            var consumerEvent = new EventingBasicConsumer(channel);

            consumerEvent.Received += (ch, ea) =>
            {
                var reportService = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IReportService>();
                var incomingModel = JsonConvert.DeserializeObject<ReportRequestData>(Encoding.UTF8.GetString(ea.Body.ToArray()));
                Console.WriteLine("Data received");
                Console.WriteLine($"Received Id: {incomingModel.ReportId}");
                reportService.CreateReportDetail(incomingModel.ReportId);
            };

            channel.BasicConsume(createDocumentQueue, true, consumerEvent);

            return app;
        }
    }
}
