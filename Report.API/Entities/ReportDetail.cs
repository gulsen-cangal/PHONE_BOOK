using Report.API.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Report.API.Entities
{
    [Table("ReportDetail")]
    public class ReportDetail:BaseEntity
    {
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }
        public Guid ReportId { get; set; }
    }
}
