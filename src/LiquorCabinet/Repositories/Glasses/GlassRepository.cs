using Dapper;
using LiquorCabinet.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquorCabinet.Repositories.Glasses
{
    internal class GlassRepository : ICrudRepository<int, Glass>
    {
        private readonly ILogger _logger;
        private readonly IDbConnectionFactory _connectionFactory;
        
        public GlassRepository(ILogger<GlassRepository> logger, IDbConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public async Task InsertAsync(Glass entityToCreate)
        {
            _logger.LogDebug("Inserting new glass...");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var rows = await connection.QueryAsync<int>(SqlScripts.InsertGlassware);
                entityToCreate.Id = rows.FirstOrDefault();
            }
        }

        public Task InsertListAsync(IEnumerable<Glass> entitiesToCreate) => throw new NotImplementedException();

        public Task<Glass> GetAsync(int id) => throw new NotImplementedException();

        public async Task<IEnumerable<Glass>> GetListAsync()
        {
            _logger.LogDebug("Retrieving all Glasses...");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var glasses = await connection.QueryAsync<Glass>(SqlScripts.GetListGlassware);
                _logger.LogInformation($"Retrieved {glasses.Count()} glasses.");
                return glasses;
            }
        }

        public Task UpdateAsync(Glass entityToUpdate) => throw new NotImplementedException();

        public Task DeleteAsync(int id) => throw new NotImplementedException();
    }
}