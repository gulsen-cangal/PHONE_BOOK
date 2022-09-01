using Microsoft.EntityFrameworkCore;
using Report.API.Data;
using Report.API.Entities.Context;
using Report.API.Enums;
using Report.API.Services.Interfaces;

namespace Report.API.Services
{
    public class ReportService : IReportService
    {
        ReportContext reportContext;
        private readonly IHttpClientFactory httpClientFactory;

        public ReportService(ReportContext context, IHttpClientFactory httpClientFactory)
        {
            reportContext = context;
            this.httpClientFactory = httpClientFactory;
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
    }
}
