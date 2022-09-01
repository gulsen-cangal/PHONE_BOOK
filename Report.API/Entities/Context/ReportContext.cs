using Microsoft.EntityFrameworkCore;

namespace Report.API.Entities.Context
{
    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions options) : base(options)
        {
        }

        protected ReportContext()
        {
        }

        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportDetail> ReportDetails { get; set; }
    }
}
