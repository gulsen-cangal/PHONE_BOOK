using PersonProcesses.API.Data;

namespace PersonProcesses.API.Services.Interfaces
{
    public interface IPersonService
    {
        Task<ReturnData> AddPerson(PersonData personData);
        Task<ReturnData> DeletePerson(Guid personUuId);
        Task<ReturnData> GetAllPersons();
        Task<ReturnData> GetPerson(Guid personId);
        Task<ReturnData> GetPersonDetail(Guid personId);
    }
}
