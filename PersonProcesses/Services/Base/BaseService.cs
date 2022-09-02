using PersonProcesses.Entities.Context;

namespace PersonProcesses.API.Services.Base
{
    public class BaseService
    {
        public readonly PersonProcessesContext context;

        public BaseService(PersonProcessesContext context)
        {
            this.context = context;
        }
    }
}
