using System.ComponentModel;

namespace Report.API.Enums
{
    public enum ReportStatusType
    {
        [Description("Hazırlanıyor")]
        Preparing,
        [Description("Tamamlandi")]
        Completed
    }
}
