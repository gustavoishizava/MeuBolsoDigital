using System.Collections.Generic;
using System.Threading.Tasks;

namespace MBD.IntegrationEventLog.Services
{
    public interface IIntegrationEventLogService
    {
        Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync();
        Task SaveEventAsync<T>(T @event, string eventTypeName) where T : class;
        Task RemoveEventAsync(IntegrationEventLogEntry integrationEventLog);
    }
}