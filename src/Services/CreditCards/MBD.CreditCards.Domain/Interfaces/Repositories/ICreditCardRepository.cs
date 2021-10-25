using System.Collections.Generic;
using System.Threading.Tasks;
using MBD.Core.Data;
using MBD.CreditCards.Domain.Entities;

namespace MBD.CreditCards.Domain.Interfaces.Repositories
{
    public interface ICreditCardRepository : IBaseRepository<CreditCard>
    {
        Task<IEnumerable<CreditCard>> GetAllAsync();
    }
}