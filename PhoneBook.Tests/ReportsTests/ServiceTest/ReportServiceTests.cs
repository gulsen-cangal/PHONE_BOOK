using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using Report.API.Constants;
using Report.API.Entities.Context;
using Report.API.Entities;
using Report.API.Enums;
using Report.API.Services;
using System.Net;
using Xunit;
using Report.API.Data;

namespace PhoneBook.Tests.ReportsTests.ServiceTest
{
    public class ReportServiceTests
    {
        private readonly IHttpClientFactory httpClientFactory;

        public static DbContextOptions<ReportContext> GetReportContextForInMemoryDb()
        {
            return new DbContextOptionsBuilder<ReportContext>()
                .UseInMemoryDatabase(databaseName: "Report" + Guid.NewGuid())
                .Options;
        }
        public static int? GetStatusCodeFromActionResult<T>(ActionResult<T> actionResult)
            => ((ObjectResult)actionResult.Result).StatusCode;
        [Fact]
        public async Task CreateNewReport_Should_Create_Report()
        {
            var context = new ReportContext(GetReportContextForInMemoryDb());
            IOptions<ReportSettings> settings = Options.Create<ReportSettings>(new ReportSettings()
            {
                PersonProcessesAPIUrl = "https://localhost:7218/api",
                RabbitMqConsumer = "amqp://guest:guest@localhost:5672",
            });

            var service = new ReportService(context, new Mock<IHttpClientFactory>().Object, settings);

            var result = await service.CreateNewReport();

            Assert.True(result.Data != null);
        }

        [Fact]
        public async Task GetAllReports_Should_Get_Reports()
        {
            var context = new ReportContext(GetReportContextForInMemoryDb());

            context.Reports.Add(new Report.API.Entities.Report()
            {
                ReportStatus = ReportStatusType.Preparing,
                ReportPath = "",
            });

            context.Reports.Add(new Report.API.Entities.Report()
            {
                ReportStatus = ReportStatusType.Completed,
                ReportPath = Directory.GetCurrentDirectory() + "\\Reports\\PHONE.BOOK.REPORT." + Guid.NewGuid() + ".csv",
            });

            await context.SaveChangesAsync();

            IOptions<ReportSettings> settings = Options.Create<ReportSettings>(new ReportSettings()
            {
                PersonProcessesAPIUrl = "https://localhost:7218/api",
                RabbitMqConsumer = "amqp://guest:guest@localhost:5672",
            });
            var service = new ReportService(context, new Mock<IHttpClientFactory>().Object, settings);


            var result = await service.GetAllReports();

            Assert.Equal(2, context.Reports.Local.Count);
        }

        [Fact]
        public async Task GetReportDetail_With_Valid_Params_Should_Get_Report_Detail()
        {
            var context = new ReportContext(GetReportContextForInMemoryDb());

            var reportId = Guid.NewGuid();
            context.Reports.Add(new Report.API.Entities.Report()
            {
                UUID = reportId,
                ReportStatus = ReportStatusType.Preparing,
                ReportPath = ""
            });

            context.ReportDetails.Add(new ReportDetail()
            {
                Location = "Çorlu",
                PersonCount = 1,
                PhoneNumberCount = 1,
                ReportId = reportId
            });

            await context.SaveChangesAsync();

            IOptions<ReportSettings> settings = Options.Create<ReportSettings>(new ReportSettings()
            {
                PersonProcessesAPIUrl = "https://localhost:7218/api",
                RabbitMqConsumer = "amqp://guest:guest@localhost:5672",
            });
            var service = new ReportService(context, new Mock<IHttpClientFactory>().Object, settings);

            var result = await service.GetReportDetail(reportId);

            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GenerateReportDocument_With_Valid_Params_Should_CreateFile()
        {
            var context = new ReportContext(GetReportContextForInMemoryDb());

            var reportId = Guid.NewGuid();
            context.Reports.Add(new Report.API.Entities.Report()
            {
                UUID = reportId,
                ReportStatus = ReportStatusType.Preparing,
                ReportPath = ""
            });

            context.ReportDetails.Add(new ReportDetail()
            {
                Location = "Çorlu",
                PersonCount = 1,
                PhoneNumberCount = 1,
                ReportId = reportId
            });

            await context.SaveChangesAsync();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{'uuid': 'a5ba5d9c-f6fa-4466-9ef6-508592438fc9','informationType':2,'informationContent':'Mersin', 'personId': 'b890413c-2dca-499a-8578-5cfb14e0b1eb'}]"),
                });

            var mockFactory = new Mock<IHttpClientFactory>();

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);


            IOptions<ReportSettings> aaaa = Options.Create<ReportSettings>(new ReportSettings()
            {
                PersonProcessesAPIUrl = "https://localhost:7218/api",
                RabbitMqConsumer = "amqp://guest:guest@localhost:5672",
            });
            ReportSettings reportSettings = new ReportSettings()
            {
                PersonProcessesAPIUrl = "https://localhost:7218/api",
                RabbitMqConsumer = "amqp://guest:guest@localhost:5672",
            };

            var request = new HttpRequestMessage(HttpMethod.Get, $"{reportSettings.PersonProcessesAPIUrl}/Person/ContactInformations");
            var response = await client.SendAsync(request);
            var responseStream = await response.Content.ReadAsStringAsync();
            var contactInformations = JsonConvert.DeserializeObject<IEnumerable<ContactInformationData>>(responseStream);

            var statisticsReport = contactInformations.Where(x => x.InformationType == 2).Select(x => x.InformationContent).Distinct().Select(x => new ReportDetail
            {
                ReportId = reportId,
                Location = x,
                PersonCount = contactInformations.Where(y => y.InformationType == 2 && y.InformationContent == x).Count(),
                PhoneNumberCount = contactInformations.Where(y => y.InformationType == 0 && contactInformations.Where(y => y.InformationType == 2 && y.InformationContent == x).Select(x => x.PersonId).Contains(y.PersonId)).Count(),
            });

            var service = new ReportService(context, new Mock<IHttpClientFactory>().Object, aaaa);

            string rPath = Directory.GetCurrentDirectory() + "\\Reports\\PHONE.BOOK.REPORT." + Guid.NewGuid() + ".csv";
            var result = await service.GenerateReportDocument(statisticsReport, rPath);

            Assert.NotNull(result.Data);
        }
    }
}
