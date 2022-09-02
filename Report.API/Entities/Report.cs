using Report.API.Entities.Base;
using Report.API.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Report.API.Entities
{
    [Table("Reports")]
    public class Report:BaseEntity
    {
        public DateTime RequestDate { get; set; }
        public ReportStatusType ReportStatus { get; set; }
        public string ReportPath { get; set; }
        public virtual List<ReportDetail> ReportDetails { get; set; }
    }
}
