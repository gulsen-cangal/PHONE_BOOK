using PersonProcesses.Entities.Base;
using PersonProcesses.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonProcesses.Entities
{
    [Table("ContactInformations")]
    public class ContactInformation:BaseEntity
    {       
        public InformationTypesEnums InformationType { get; set; }
        public string InformationContent { get; set; }
        public Guid PersonUUId { get; set; }
    }
}
