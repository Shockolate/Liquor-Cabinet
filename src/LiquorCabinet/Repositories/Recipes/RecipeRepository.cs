using Dapper;
using LiquorCabinet.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquorCabinet.Repositories.Recipes
{
    internal class RecipeRepository : IRecipeRepository
    {
        private readonly ILogger _logger;
        private readonly IDbConnectionFactory _connectionFactory;

        internal RecipeRepository(ILogger<RecipeRepository> logger, IDbConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public async Task InsertAsync(Recipe entityToCreate)
        {
            _logger.LogInformation($"Inserting new recipe: {entityToCreate.Name}");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var recipeId = await connection.QueryAsync<int>(SqlScripts.InsertRecipe,
                    new {entityToCreate.Name, entityToCreate.Instructions, GlasswareId = entityToCreate.Glassware.Id});
                await connection.ExecuteAsync(SqlScripts.InsertRecipeComponent,
                    entityToCreate.Components.Select(c => new {RecipeId = recipeId, c.ComponentId, c.QuantityPart, c.QuantityMetric, c.QuantityImperial}));
            }
        }

        public Task InsertListAsync(IEnumerable<Recipe> entitiesToCreate)
        {
            throw new NotImplementedException();
        }

        public async Task<Recipe> GetAsync(int id)
        {
            _logger.LogInformation($"Getting recipe for {id}");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var rows = await connection.QueryAsync<RecipeRow>(SqlScripts.GetRecipe, new {RecipeId = id});
                var recipeRows = rows as RecipeRow[] ?? rows.ToArray();
                if (!recipeRows.Any())
                {
                    throw new EntityNotFoundException("Recipe", id);
                }
                return ConvertRecipeRowsToRecipes(recipeRows).Single();
            }
        }

        public async Task<IEnumerable<Recipe>> GetListAsync()
        {
            _logger.LogInformation("Getting all recipes...");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var rows = await connection.QueryAsync<RecipeRow>(SqlScripts.GetAllRecipes);
                return ConvertRecipeRowsToRecipes(rows);
            }
        }

        public async Task UpdateAsync(Recipe entityToUpdate)
        {
            _logger.LogInformation($"Updating Recipe: {entityToUpdate.Id}");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var rows = await connection.ExecuteAsync(SqlScripts.UpdateRecipe,
                    new {RecipeId = entityToUpdate.Id, entityToUpdate.Name, entityToUpdate.Instructions, GlasswareId = entityToUpdate.Glassware.Id});
                if (rows == 0)
                {
                    throw new EntityNotFoundException("Recipe", entityToUpdate.Id);
                }
            }
        }

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<IEnumerable<Recipe>> GetRecipeListForUserAsync(int userId) => throw new NotImplementedException();

        public async Task AddComponentsToRecipeAsync(int recipeId, IEnumerable<RecipeComponent> recipeComponents)
        {
            var recipeComponentsArray = recipeComponents.ToArray();
            _logger.LogInformation($"Adding Components: {recipeComponentsArray.Select(rc => $"{rc.ComponentId}, ")} to Recipe: {recipeId}");
            using (var connection = _connectionFactory.CreateLiquorDbConnection())
            {
                connection.Open();
                var sqlParameters = recipeComponentsArray.Select(rc =>
                    new {RecipeId = recipeId, rc.ComponentId, rc.QuantityPart, rc.QuantityMetric, rc.QuantityImperial});
                var rows = await connection.ExecuteAsync(SqlScripts.InsertRecipeComponent, sqlParameters);

                if (rows != recipeComponentsArray.Length)
                {
                    throw new Exception("Rows affected does not match Recipe Components.");
                }
            }
        }

        internal static IEnumerable<Recipe> ConvertRecipeRowsToRecipes(IEnumerable<RecipeRow> recipeRows)
        {
            var recipeRowsArray = recipeRows as RecipeRow[] ?? recipeRows.ToArray();
            IDictionary<int, Recipe> recipeDictionary = new Dictionary<int, Recipe>(recipeRowsArray.Length);
            foreach (var recipeRow in recipeRowsArray)
            {
                if (!recipeDictionary.ContainsKey(recipeRow.RecipeId))
                {
                    recipeDictionary.Add(recipeRow.RecipeId,
                        new Recipe
                        {
                            Id = recipeRow.RecipeId,
                            Instructions = recipeRow.RecipeInstructions,
                            Name = recipeRow.RecipeName,
                            Glassware =
                                new Glass
                                {
                                    Id = recipeRow.GlasswareId,
                                    Name = recipeRow.GlasswareName,
                                    Description = recipeRow.GlasswareDescription,
                                    TypicalSize = recipeRow.GlasswareTypicalSize
                                },
                            Components = new List<RecipeComponent>
                            {
                                new RecipeComponent
                                {
                                    Id = recipeRow.RecipeComponentQuantityId,
                                    ComponentId = recipeRow.ComponentId,
                                    ComponentName = recipeRow.ComponentName,
                                    QuantityPart = recipeRow.ComponentQuantityPart,
                                    QuantityMetric = recipeRow.ComponentQuantityMetric,
                                    QuantityImperial = recipeRow.ComponentQuantityImperial,
                                    RecipeId = recipeRow.RecipeId
                                }
                            }
                        });
                }
                else
                {
                    recipeDictionary[recipeRow.RecipeId].Components.Add(new RecipeComponent
                    {
                        ComponentId = recipeRow.ComponentId,
                        ComponentName = recipeRow.ComponentName,
                        Id = recipeRow.RecipeComponentQuantityId,
                        QuantityImperial = recipeRow.ComponentQuantityImperial,
                        QuantityMetric = recipeRow.ComponentQuantityMetric,
                        QuantityPart = recipeRow.ComponentQuantityPart,
                        RecipeId = recipeRow.RecipeId
                    });
                }
            }

            foreach (var kvp in recipeDictionary)
            {
                yield return kvp.Value;
            }
        }
    }
}