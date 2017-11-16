using System.Collections.Generic;
using System.Threading.Tasks;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.Repositories
{
    internal interface ICrudRepository<T, in TId> where T : EntityBase<TId>
    {
        Task InsertAsync(T entityToCreate, ILogger logger);
        Task<T> GetAsync(TId id, ILogger logger);
        Task<IEnumerable<T>> GetListAsync(ILogger logger);
        Task UpdateAsync(T entityToUpdate, ILogger logger);
        Task DeleteAsync(TId id, ILogger logger);
    }
}