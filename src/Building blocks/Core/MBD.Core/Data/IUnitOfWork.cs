using System.Threading.Tasks;

namespace MBD.Core.Data
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}