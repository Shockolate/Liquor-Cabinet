using LiquorCabinet.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace LiquorCabinet.Repositories.Ingredients
{
    internal class IngredientRepository : ICrudRepository<int, Ingredient>
    {
        private readonly ILogger _logger;
        private readonly IDbConnectionFactory _connectionFactory;
        
        public IngredientRepository(ILogger<IngredientRepository> logger, IDbConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public async Task InsertAsync(Ingredient entityToCreate)
        {
            _logger.LogDebug("Inserting a new Ingredient...");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var rows = await connection.QueryAsync<int>(SqlScripts.InsertIngredient);
                entityToCreate.Id = rows.SingleOrDefault();
            }
        }

        public Task InsertListAsync(IEnumerable<Ingredient> entitiesToCreate) => throw new NotImplementedException();

        public async Task<Ingredient> GetAsync(int id)
        {
            _logger.LogDebug($"Getting Ingredient: {id}");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var rows = await connection.QueryAsync<Ingredient>(SqlScripts.GetIngredient).ConfigureAwait(false);
                return rows.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Ingredient>> GetListAsync()
        {
            _logger.LogDebug("Getting all Ingredients.");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var ingredients = await connection.QueryAsync<Ingredient>(SqlScripts.GetListIngredient);
                _logger.LogDebug($"Retrieved {ingredients.Count()} Ingredients.");
                return ingredients;
            }
        }

        public async Task UpdateAsync(Ingredient entityToUpdate)
        {
            _logger.LogDebug($"Updating Ingredient: {entityToUpdate.Id}");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var count = await connection.ExecuteAsync(SqlScripts.UpdateIngredient);
                if (count != 1)
                {
                    throw new EntityNotFoundException("Ingredient", entityToUpdate.Id);
                }
            }
        }

        public async Task DeleteAsync(int id, ILogger logger)
        {
            _logger.LogDebug($"Deleting Ingredient: {id}");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var count = await connection.ExecuteAsync(SqlScripts.DeleteIngredient);
                if (count != 1)
                {
                    throw new EntityNotFoundException("Ingredient", id);
                }
            }
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}