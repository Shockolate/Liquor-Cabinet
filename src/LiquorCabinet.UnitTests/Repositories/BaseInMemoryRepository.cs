using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiquorCabinet.Repositories;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Repositories
{
    public abstract class BaseInMemoryRepository<T, TOfId> : ICrudRepository<T, TOfId> where T : EntityBase<TOfId>
    {
        private readonly bool _throws;

        protected BaseInMemoryRepository() : this(false) { }

        protected BaseInMemoryRepository(bool throws)
        {
            _throws = throws;
        }

        private void ThrowCheck()
        {
            if (_throws)
            {
                throw new Exception("Repository Error.");
            }
        }

        public Task DeleteAsync(TOfId id, ILogger logger)
        {
            ThrowCheck();
            return DoDeleteAsync(id, logger);
        }

        protected abstract Task DoDeleteAsync(TOfId id, ILogger logger);

        public Task<T> GetAsync(TOfId id, ILogger logger)
        {
            ThrowCheck();
            return DoGetAsync(id, logger);
        }

        protected abstract Task<T> DoGetAsync(TOfId id, ILogger logger);

        public Task<IEnumerable<T>> GetListAsync(ILogger logger)
        {
            ThrowCheck();
            return DoGetListAsync(logger);
        }

        protected abstract Task<IEnumerable<T>> DoGetListAsync(ILogger logger);

        public Task InsertAsync(T entityToCreate, ILogger logger)
        {
            ThrowCheck();
            return DoInsertAsync(entityToCreate, logger);
        }

        protected abstract Task DoInsertAsync(T entityToCreate, ILogger logger);

        public Task UpdateAsync(T entityToUpdate, ILogger logger)
        {
            ThrowCheck();
            return DoUpdateAsync(entityToUpdate, logger);
        }

        protected abstract Task DoUpdateAsync(T entityToUpdate, ILogger logger);
    }
}
