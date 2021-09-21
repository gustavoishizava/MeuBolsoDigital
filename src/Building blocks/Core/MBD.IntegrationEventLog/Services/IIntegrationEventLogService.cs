using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace MBD.IntegrationEventLog.Services
{
    public interface IIntegrationEventLogService
    {
        Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync();
        Task SaveEventAsync<T>(T @event, IDbContextTransaction transaction) where T : class;
        Task RemoveEventAsync(IntegrationEventLogEntry integrationEventLog);
    }
}