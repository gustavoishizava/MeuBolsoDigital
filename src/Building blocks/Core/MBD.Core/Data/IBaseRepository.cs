using System;
using System.Threading.Tasks;
using MBD.Core.Entities;

namespace MBD.Core.Data
{
    public interface IBaseRepository<TEntity> where TEntity : IAggregateRoot
    {
        Task<TEntity> GetByIdAsync(Guid id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}