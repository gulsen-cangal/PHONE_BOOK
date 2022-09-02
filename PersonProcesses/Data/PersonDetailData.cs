namespace PersonProcesses.API.Data
{
    public class PersonDetailData
    {
        public PersonData Person { get; set; }
        public List<ContactInformationData> ContactInformations { get; set; } = new();
    }
}
