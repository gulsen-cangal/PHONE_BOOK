using PersonProcesses.Enums;

namespace PersonProcesses.API.Data
{
    public class ContactInformationData
    {
        public Guid UUID { get; set; }
        public InformationTypesEnums InformationType { get; set; }
        public string InformationContent { get; set; }
        public Guid PersonId { get; set; }
    }
}
