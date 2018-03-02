using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiquorCabinet.Repositories
{
    public interface ICrudRepository<in TId, T> where T : EntityBase<TId>
    {
        Task InsertAsync(T entityToCreate);
        Task InsertListAsync(IEnumerable<T> entitiesToCreate);
        Task<T> GetAsync(TId id);
        Task<IEnumerable<T>> GetListAsync();
        Task UpdateAsync(T entityToUpdate);
        Task DeleteAsync(TId id);
    }
}