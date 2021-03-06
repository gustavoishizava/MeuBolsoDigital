using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MBD.IntegrationEventLog.Services
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly IntegrationEventLogContext _context;

        public IntegrationEventLogService(IntegrationEventLogContext context)
        {
            _context = context;
        }

        public async Task RemoveEventAsync(IntegrationEventLogEntry integrationEventLog)
        {
            _context.Remove(integrationEventLog);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync()
        {
            return await _context.IntegrationEventLogs
                            .AsNoTracking()
                            .OrderBy(x => x.CreatedAt)
                            .ToListAsync();
        }

        public async Task SaveEventAsync<T>(T @event, string eventTypeName) where T : class
        {
            var content = JsonSerializer.Serialize(@event);
            var integrationEventLog = new IntegrationEventLogEntry(eventTypeName, content);

            _context.Add(integrationEventLog);

            await _context.SaveChangesAsync();
        }
    }
}