using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Report.API.Constants;
using Report.API.Data;
using Report.API.Services.Interfaces;

namespace Report.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportSettings reportSettings;
        private readonly IReportService reportService;

        public ReportController(IReportService reportService, IOptions<ReportSettings> reportSettings)
        {
            this.reportService = reportService;
            this.reportSettings = reportSettings?.Value;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReportReturnData>> CreateNewReport()
        {
            var result = await reportService.CreateNewReport();

            if (result.Response == true)
            {
                var model = new ReportRequestData()
                {
                    ReportId = ((Report.API.Entities.Report)result.Data).UUID
                };
                await reportService.CreateRabbitMQPublisher(model, reportSettings);
                return Accepted("", result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReportReturnData>> GetAllReports()
        {
            var allReports = await reportService.GetAllReports();

            if (allReports.Response == false)
            {
                return NotFound(allReports.Message);
            }
            else
            {
                return Ok(allReports);
            }
        }

        [HttpGet("{reportId}/Detail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReportReturnData>> GetReportDetail(Guid reportId)
        {
            var result = await reportService.GetReportDetail(reportId);

            if (result.Response == false)
            {
                return NotFound(result.Message);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
