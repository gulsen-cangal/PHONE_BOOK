namespace Report.API.Data
{
    public class ReportDetailData
    {
        public ReportData ReportData { get; set; }
        public List<DetailData> DetailData { get; set; } = new();
    }
}
