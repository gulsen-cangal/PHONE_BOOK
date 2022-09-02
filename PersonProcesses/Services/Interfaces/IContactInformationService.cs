using PersonProcesses.API.Data;

namespace PersonProcesses.API.Services.Interfaces
{
    public interface IContactInformationService
    {
        Task<ReturnData> AddContactInformation(Guid personId, ContactInformationData contactInformationData);
        Task<ReturnData> DeleteContactInformation(Guid contactInformationId);
        Task<ReturnData> GetAllContactInformations();
    }
}
