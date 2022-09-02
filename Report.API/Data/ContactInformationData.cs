namespace Report.API.Data
{
    public class ContactInformationData
    {
        public Guid UUID { get; set; }
        public int InformationType { get; set; }
        public string InformationContent { get; set; }
        public Guid PersonId { get; set; }
    }
}
