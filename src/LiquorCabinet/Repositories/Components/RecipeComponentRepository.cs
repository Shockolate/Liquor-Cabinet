using Dapper;
using LiquorCabinet.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquorCabinet.Repositories.Components
{
    internal class RecipeComponentRepository : ICrudRepository<int, RecipeComponent>
    {
        private readonly ILogger _logger;
        private readonly IDbConnectionFactory _connectionFactory;

        internal RecipeComponentRepository(ILogger<RecipeComponentRepository> logger, IDbConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public Task InsertAsync(RecipeComponent entityToCreate) => throw new NotImplementedException();
        public Task InsertListAsync(IEnumerable<RecipeComponent> entitiesToCreate) => throw new NotImplementedException();

        public async Task<RecipeComponent> GetAsync(int id)
        {
            _logger.LogInformation($"Retrieving RecipeComponent: {id}");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var componentRows = await connection.QueryAsync<RecipeComponent>(SqlScripts.GetRecipeComponent, new {RecipeComponentQuantityId = id});

                try
                {
                    return componentRows.Single();
                }
                catch (Exception)
                {
                    throw new EntityNotFoundException($"RecipeComponent: {id}");
                }
            }
        }

        public Task<IEnumerable<RecipeComponent>> GetListAsync() => throw new NotImplementedException();

        public async Task UpdateAsync(RecipeComponent entityToUpdate)
        {
            _logger.LogInformation($"Updating RecipeComponent: {entityToUpdate.Id}");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var rows = await connection.ExecuteAsync(SqlScripts.UpdateRecipeComponent,
                    new
                    {
                        entityToUpdate.QuantityPart,
                        entityToUpdate.QuantityMetric,
                        entityToUpdate.QuantityImperial,
                        RecipComponentQuantityId = entityToUpdate.Id,
                        entityToUpdate.ComponentId,
                        entityToUpdate.RecipeId
                    });
                if (rows == 0)
                {
                    throw new ArgumentException("0 Rows updated.");
                }
            }
        }

        public Task DeleteAsync(int id) => throw new NotImplementedException();
    }
}