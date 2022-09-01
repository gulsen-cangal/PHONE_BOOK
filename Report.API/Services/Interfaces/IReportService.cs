using Report.API.Data;

namespace Report.API.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportReturnData> CreateNewReport();
        Task<ReportReturnData> GetAllReports();
        Task<ReportReturnData> GetReportDetail(Guid reportId);
    }
}
