using Report.API.Entities.Base;

namespace Report.API.Data
{
    public class DetailData:BaseEntity
    {
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }
    }
}
