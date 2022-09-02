using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Report.API.Constants;
using Report.API.Controllers;
using Report.API.Data;
using Report.API.Entities.Context;
using Report.API.Services.Interfaces;
using Xunit;

namespace PhoneBook.Tests.ReportsTests.ControllerTest
{
    public class ReportControllerTests
    {
        public static DbContextOptions<ReportContext> GetReportContextForInMemoryDb()
        {
            return new DbContextOptionsBuilder<ReportContext>()
                .UseInMemoryDatabase(databaseName: "Report" + Guid.NewGuid())
                .Options;
        }
        public static int? GetStatusCodeFromActionResult<T>(ActionResult<T> actionResult)
            => ((ObjectResult)actionResult.Result).StatusCode;

        [Fact]
        public async Task GetAllReport_With_Invalid_Params_Should_Return_404()
        {
            var mockReportService = new Mock<IReportService>();
            mockReportService
                .Setup(x => x.GetAllReports())
                .ReturnsAsync(() => new ReportReturnData()
                {
                    Response = false
                });

            var reportController = new ReportController(mockReportService.Object, new Mock<IOptions<ReportSettings>>().Object);

            var result = await reportController.GetAllReports();

            Assert.Equal(404, GetStatusCodeFromActionResult(result));

        }

        [Fact]
        public async Task GetAllReport_With_Valid_Params_Should_Return_200()
        {
            var mockPeportService = new Mock<IReportService>();
            mockPeportService
                .Setup(x => x.GetAllReports())
                .ReturnsAsync(() => new ReportReturnData()
                {
                    Response = true
                });

            var reportController = new ReportController(mockPeportService.Object, new Mock<IOptions<ReportSettings>>().Object);

            var result = await reportController.GetAllReports();

            Assert.Equal(200, GetStatusCodeFromActionResult(result));
        }


        [Fact]
        public async Task GetReportDetailWithValidParamsReturn200()
        {
            var guid = Guid.NewGuid();
            var mockPeportService = new Mock<IReportService>();
            mockPeportService
                .Setup(x => x.GetReportDetail(guid))
                .ReturnsAsync(() => new ReportReturnData()
                {
                    Response = true
                });

            var reportController = new ReportController(mockPeportService.Object, new Mock<IOptions<ReportSettings>>().Object);

            var result = await reportController.GetReportDetail(guid);

            Assert.Equal(200, GetStatusCodeFromActionResult(result));
        }

        [Fact]
        public async Task GetReportDetail_With_Invalid_Params_Should_Return_404()
        {
            var mockPeportService = new Mock<IReportService>();
            mockPeportService
                .Setup(x => x.GetReportDetail(It.IsAny<Guid>()))
                .ReturnsAsync(() => new ReportReturnData()
                {
                    Response = false
                });

            var reportController = new ReportController(mockPeportService.Object, new Mock<IOptions<ReportSettings>>().Object);

            var result = await reportController.GetReportDetail(Guid.Empty);

            Assert.Equal(404, GetStatusCodeFromActionResult(result));
        }
    }
}
