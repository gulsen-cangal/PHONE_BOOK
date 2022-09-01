using Report.API.Entities.Base;
using Report.API.Enums;

namespace Report.API.Data
{
    public class ReportData:BaseEntity
    {
        public DateTime RequestDate { get; set; }
        public ReportStatusType ReportStatus { get; set; }
        public string ReportPath { get; set; }
    }
}
