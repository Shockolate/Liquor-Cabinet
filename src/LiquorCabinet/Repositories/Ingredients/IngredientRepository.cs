using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.Repositories.Ingredients
{
    internal class IngredientRepository : ICrudRepository<Ingredient, int>
    {
        private readonly Func<ILogger, IDbConnection> _connectionFactory;
        public IngredientRepository() : this(ConnectionFactory.CreateLiquorDbConnection) { }

        public IngredientRepository(Func<ILogger, IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task InsertAsync(Ingredient entityToCreate, ILogger logger)
        {
            logger.LogDebug(() => "Inserting a new Ingredient...");
            using (var connection = _connectionFactory.Invoke(logger))
            {
                connection.Open();
                var rows = await connection.QueryAsync<int>(SqlScripts.InsertIngredient);
                entityToCreate.Id = rows.SingleOrDefault();
            }
        }

        public async Task<Ingredient> GetAsync(int id, ILogger logger)
        {
            logger.LogDebug(() => $"Getting Ingredient: {id}");
            using (var connection = _connectionFactory.Invoke(logger))
            {
                connection.Open();
                var rows = await connection.QueryAsync<Ingredient>(SqlScripts.GetIngredient).ConfigureAwait(false);
                return rows.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Ingredient>> GetListAsync(ILogger logger)
        {
            logger.LogDebug(() => "Getting all Ingredients.");
            using (var connection = _connectionFactory.Invoke(logger))
            {
                connection.Open();
                var ingredients = await connection.QueryAsync<Ingredient>(SqlScripts.GetListIngredient);
                logger.LogDebug(() => $"Retrieved {ingredients.Count()} Ingredients.");
                return ingredients;
            }
        }

        public async Task UpdateAsync(Ingredient entityToUpdate, ILogger logger)
        {
            logger.LogDebug(() => $"Updating Ingredient: {entityToUpdate.Id}");
            using (var connection = _connectionFactory.Invoke(logger))
            {
                connection.Open();
                var count = await connection.ExecuteAsync(SqlScripts.UpdateIngredient);
                if (count != 1)
                {
                    throw new EntityNotFoundException($"Ingredient: {entityToUpdate.Id} Not Found.");
                }
            }
        }

        public async Task DeleteAsync(int id, ILogger logger)
        {
            logger.LogDebug(() => $"Deleting Ingredient: {id}");
            using (var connection = _connectionFactory.Invoke(logger))
            {
                connection.Open();
                var count = await connection.ExecuteAsync(SqlScripts.DeleteIngredient);
                if (count != 1)
                {
                    throw new EntityNotFoundException($"Ingredient: {id} Not Found.");
                }
            }
        }
    }
}