using System.ComponentModel;

namespace PersonProcesses.Enums
{
    public enum InformationTypesEnums
    {
        [Description("Telefon Numarası")]
        PhoneNumber,
        [Description("E-Mail Adresi")]
        EmailAddress,
        [Description("Konum Bilgisi")]
        Location
    }
}
