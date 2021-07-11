using System;

namespace MBD.Core.Entities
{
    public interface ITenant
    {
        Guid TenantId { get; }
    }
}