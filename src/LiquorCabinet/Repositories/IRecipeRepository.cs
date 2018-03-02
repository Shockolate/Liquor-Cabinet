using System.Collections.Generic;
using System.Threading.Tasks;
using LiquorCabinet.Models;

namespace LiquorCabinet.Repositories
{
    public interface IRecipeRepository : ICrudRepository<int, Recipe>
    {
        Task<IEnumerable<Recipe>> GetRecipeListForUserAsync(int userId);
        Task AddComponentsToRecipeAsync(int recipeId, IEnumerable<RecipeComponent> recipeComponents);
    }
}