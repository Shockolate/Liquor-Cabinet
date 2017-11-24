using System.Collections.Generic;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.Repositories.Recipes
{
    internal interface IRecipeRepository : ICrudRepository<Recipe, int>
    {
        IEnumerable<Recipe> GetRecipeListForUserAsync(int userId, ILogger logger);
    }
}