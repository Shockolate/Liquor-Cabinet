using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiquorCabinet.Repositories;


namespace LiquorCabinet.UnitTests.Repositories
{
    public abstract class BaseInMemoryRepository<TOfId, T> : ICrudRepository<TOfId, T> where T : EntityBase<TOfId>
    {
        private readonly bool _throws;

        protected BaseInMemoryRepository() : this(false) { }

        protected BaseInMemoryRepository(bool throws)
        {
            _throws = throws;
        }

        public Task DeleteAsync(TOfId id)
        {
            ThrowCheck();
            return DoDeleteAsync(id);
        }

        public Task InsertListAsync(IEnumerable<T> entitiesToCreate)
        {
            ThrowCheck();
            return DoInsertListAsync(entitiesToCreate);
        }

        public Task<T> GetAsync(TOfId id)
        {
            ThrowCheck();
            return DoGetAsync(id);
        }

        public Task<IEnumerable<T>> GetListAsync()
        {
            ThrowCheck();
            return DoGetListAsync();
        }

        public Task InsertAsync(T entityToCreate)
        {
            ThrowCheck();
            return DoInsertAsync(entityToCreate);
        }

        public Task UpdateAsync(T entityToUpdate)
        {
            ThrowCheck();
            return DoUpdateAsync(entityToUpdate);
        }

        protected void ThrowCheck()
        {
            if (_throws)
            {
                throw new Exception("Repository Error.");
            }
        }

        protected abstract Task DoDeleteAsync(TOfId id);

        protected abstract Task DoInsertListAsync(IEnumerable<T> entitesToCreate);

        protected abstract Task<T> DoGetAsync(TOfId id);

        protected abstract Task<IEnumerable<T>> DoGetListAsync();

        protected abstract Task DoInsertAsync(T entityToCreate);

        protected abstract Task DoUpdateAsync(T entityToUpdate);
    }
}