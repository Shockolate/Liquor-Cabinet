using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.Repositories.Glasses
{
    internal class GlassRepository : ICrudRepository<Glass, int>
    {
        private readonly Func<ILogger, IDbConnection> _connectionFactory;

        public GlassRepository() : this(ConnectionFactory.CreateLiquorDbConnection) { }

        public GlassRepository(Func<ILogger, IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task InsertAsync(Glass entityToCreate, ILogger logger)
        {
            logger.LogDebug(() => "Inserting new glass...");
            using (var connection = _connectionFactory.Invoke(logger))
            {
                connection.Open();
                var rows = await connection.QueryAsync<int>(SqlScripts.InsertGlassware);
                entityToCreate.Id = rows.FirstOrDefault();
            }
        }

        public Task<Glass> GetAsync(int id, ILogger logger) => throw new NotImplementedException();

        public async Task<IEnumerable<Glass>> GetListAsync(ILogger logger)
        {
            logger.LogDebug(() => "Retrieving all Glasses...");
            using (var connection = _connectionFactory.Invoke(logger))
            {
                connection.Open();
                var glasses = await connection.QueryAsync<Glass>(SqlScripts.GetListGlassware);
                logger.LogInfo(() => $"Retrieved {glasses.Count()} glasses.");
                return glasses;
            }
        }

        public Task UpdateAsync(Glass entityToUpdate, ILogger logger) => throw new NotImplementedException();

        public Task DeleteAsync(int id, ILogger logger) => throw new NotImplementedException();
    }
}