using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.Repositories.Recipes
{
    internal class RecipeRepository : IRecipeRepository
    {
        public async Task InsertAsync(Recipe entityToCreate, ILogger logger)
        {
            logger.LogInfo(() => $"Inserting new recipe: {entityToCreate.Name}");
            using (var connection = ConnectionFactory.CreateLiquorDbConnection(logger))
            {
                connection.Open();
                var recipeId = await connection.QueryAsync<int>(SqlScripts.InsertRecipe,
                    new {entityToCreate.Name, entityToCreate.Instructions, GlasswareId = entityToCreate.Glassware.Id});
                await connection.ExecuteAsync(SqlScripts.InsertRecipeComponentQuery,
                    entityToCreate.Components.Select(c =>
                        new {RecipeId = recipeId, c.ComponentId, c.QuantityPart, c.QuantityMetric, c.QuantityImperial}));
            }
        }

        public Task<Recipe> GetAsync(int id, ILogger logger) => throw new NotImplementedException();

        public async Task<IEnumerable<Recipe>> GetListAsync(ILogger logger)
        {
            logger.LogInfo(() => "Getting all recipes...");
            using (var connection = ConnectionFactory.CreateLiquorDbConnection(logger))
            {
                connection.Open();
                var rows = await connection.QueryAsync<RecipeRow>(SqlScripts.GetAllRecipes);
                return ConvertRecipeRowsToRecipes(rows);
            }
        }

        public Task UpdateAsync(Recipe entityToUpdate, ILogger logger) => throw new NotImplementedException();

        public Task DeleteAsync(int id, ILogger logger) => throw new NotImplementedException();

        public IEnumerable<Recipe> GetRecipeListForUserAsync(int userId, ILogger logger) => throw new NotImplementedException();

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