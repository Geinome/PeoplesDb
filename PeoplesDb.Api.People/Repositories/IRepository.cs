using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeoplesDb.Api.People.Repositories
{
    public interface IRepository<TEntity, TId>
        where TEntity : class, new()
        where TId: struct
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetAsync(TId id);

        Task<TId> AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task<TEntity> RemoveAsync(TId id);
    }
}
