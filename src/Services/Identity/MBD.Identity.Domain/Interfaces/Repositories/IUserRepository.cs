using System.Threading.Tasks;
using MBD.Core.Data;
using MBD.Identity.Domain.Entities;

namespace MBD.Identity.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IUnitOfWork
    {
        Task<User> GetByEmailAsync(string email);
        void Add(User user);
        void AddRefreshToken(RefreshToken refreshToken);
    }
}