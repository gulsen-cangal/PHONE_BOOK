
using Microsoft.EntityFrameworkCore;

namespace PersonProcesses.Entities.Context
{
    public class PersonProcessesContext:DbContext
    {
        public PersonProcessesContext(DbContextOptions options) : base(options)
        {
        }

        protected PersonProcessesContext()
        {
        }

        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<ContactInformation> ContactInformations { get; set; }
    }
}
