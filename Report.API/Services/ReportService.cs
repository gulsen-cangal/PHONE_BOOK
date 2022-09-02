using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Report.API.Constants;
using Report.API.Data;
using Report.API.Entities;
using Report.API.Entities.Context;
using Report.API.Enums;
using Report.API.Services.Interfaces;
using System.Text;

namespace Report.API.Services
{
    public class ReportService : IReportService
    {
        ReportContext reportContext;

        private readonly IHttpClientFactory httpClientFactory;
        private readonly ReportSettings reportSettings;

        public ReportService(ReportContext context, IHttpClientFactory httpClientFactory, IOptions<ReportSettings> reportSettings)
        {
            reportContext = context;
            this.httpClientFactory = httpClientFactory;
            this.reportSettings = reportSettings?.Value;
        }

        public async Task<ReportReturnData> CreateNewReport()
        {
            var report = new Report.API.Entities.Report()
            {
                RequestDate = DateTime.UtcNow,
                ReportStatus = ReportStatusType.Preparing,
                ReportPath = "",
            };

            await reportContext.Reports.AddAsync(report);
            await reportContext.SaveChangesAsync();

            if (report.UUID != Guid.Empty)
            {
                return new ReportReturnData
                {
                    Response = true,
                    Message = "Rapor kaydı eklendi.",
                    Data = report
                };
            }
            else
            {
                return new ReportReturnData
                {
                    Response = false,
                    Message = "Rapor kaydı eklenemedi.",
                    Data = null
                };
            }
        }

        public async Task<ReportReturnData> GetAllReports()
        {

            var allReports = await reportContext.Reports.Select(p => new ReportData
            {
                UUID = p.UUID,
                ReportStatus = p.ReportStatus,
                RequestDate = p.RequestDate,
                ReportPath = p.ReportPath,
            }).ToListAsync();

            if (allReports.Count == 0)
            {
                return new ReportReturnData
                {
                    Response = false,
                    Message = "Listelenecek rapor bulunmamaktadır.",
                    Data = null
                };
            }
            else
            {
                return new ReportReturnData
                {
                    Response = true,
                    Message = "Raporlar listelenmektedir.",
                    Data = allReports
                };
            }
        }

        public async Task<ReportReturnData> GetReportDetail(Guid reportId)
        {
            var result = await reportContext.Reports.Where(p => p.UUID == reportId).Select(p => new ReportDetailData
            {
                ReportData = new ReportData()
                {
                    UUID = p.UUID,
                    ReportStatus = p.ReportStatus,
                    RequestDate = p.RequestDate,
                    ReportPath = p.ReportPath,
                },
                DetailData = p.ReportDetails.Select(d => new DetailData
                {
                    UUID = d.UUID,
                    Location = d.Location,
                    PersonCount = d.PersonCount,
                    PhoneNumberCount = d.PhoneNumberCount
                }).ToList()
            }).FirstOrDefaultAsync();


            return new ReportReturnData
            {
                Response = true,
                Message = "Rapor listelenmektedir.",
                Data = result
            };

        }

        public async Task<ReportReturnData> CreateReportDetail(Guid reportId)
        {
            var report = await reportContext.Reports.Where(x => x.UUID == reportId).FirstOrDefaultAsync();

            if (report == null)
            {
                return new ReportReturnData
                {
                    Response = true,
                    Message = "Rapor kaydı bulunamadı.",
                    Data = null
                };
            }

            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{reportSettings.PersonProcessesAPIUrl}/Person/ContactInformations");
            var response = await client.SendAsync(request);

            var responseStream = await response.Content.ReadAsStringAsync();
            var contactInformations = JsonConvert.DeserializeObject<IEnumerable<Report.API.Data.ContactInformationData>>(JsonConvert.SerializeObject((JsonConvert.DeserializeObject<ReportReturnData>(responseStream)).Data));

            var statisticsReport = contactInformations.Where(x => x.InformationType == 2).Select(x => x.InformationContent).Distinct().Select(x => new ReportDetail
            {
                ReportId = reportId,
                Location = x,
                PersonCount = contactInformations.Where(y => y.InformationType == 2 && y.InformationContent == x).Count(),
                PhoneNumberCount = contactInformations.Where(y => y.InformationType == 0 && contactInformations.Where(y => y.InformationType == 2 && y.InformationContent == x).Select(x => x.PersonId).Contains(y.PersonId)).Count(),
            });
            var reportPath = Directory.GetCurrentDirectory() + "\\Reports\\PHONE.BOOK.REPORT." + Guid.NewGuid() + ".csv";
            await GenerateReportDocument(statisticsReport, reportPath);

            report.ReportStatus = ReportStatusType.Completed;
            report.ReportPath = reportPath;
            await reportContext.ReportDetails.AddRangeAsync(statisticsReport);
            await reportContext.SaveChangesAsync();
            return new ReportReturnData
            {
                Response = true,
                Message = "Rapor tamamlandı. Oluşturulan rapor dokümanı: " + reportPath,
                Data = report
            };
        }

        public async Task CreateRabbitMQPublisher(ReportRequestData reportRequestData, ReportSettings reportSettings)
        {
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

            channel.BasicPublish(documentCreateExchange, createDocumentQueue, null, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reportRequestData)));

        }

        public async Task<ReportReturnData> GenerateReportDocument(IEnumerable<ReportDetail> reportDetailList, string rPath)
        {
            var builder = new StringBuilder();
            builder.AppendLine("ReportId;Location;PersonCount;PhoneNumberCount");

            var folder = Directory.GetCurrentDirectory() + "\\Reports";

            if (!Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            foreach (var reportRecord in reportDetailList)
            {
                builder.AppendLine($"{reportRecord.ReportId};{reportRecord.Location};{reportRecord.PersonCount};{reportRecord.PhoneNumberCount}");

            }

            File.WriteAllText(rPath, builder.ToString());

            builder.Clear();
            return new ReportReturnData
            {
                Response = true,
                Message = "Rapor tamamlandı. Oluşturulan rapor dokümanı: " + rPath,
                Data = rPath
            };
        }

    }
}
