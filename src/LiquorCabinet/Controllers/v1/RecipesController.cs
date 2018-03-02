using LiquorCabinet.Dtos;
using LiquorCabinet.Models;
using LiquorCabinet.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LiquorCabinet.Controllers.v1
{
    [Route("v1/recipes")]
    public class RecipesController : Controller
    {
        private readonly ILogger _logger;
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICrudRepository<int, Glass> _glasswareRepository;

        public RecipesController(ILogger<RecipesController> logger, IRecipeRepository recipeRepository, ICrudRepository<int,Glass> glasswareRepository)
        {
            _logger = logger;
            _recipeRepository = recipeRepository;
            _glasswareRepository = glasswareRepository;
        }

        // GET: v1/recipes
        [HttpGet("")]
        public async Task<IActionResult> GetRecipes([FromQuery(Name = "userId")] string userId)
        {
            if(!string.IsNullOrEmpty(userId))
            {
                throw new NotImplementedException();
            }

            var recipes = await _recipeRepository.GetListAsync().ConfigureAwait(false);
            return Ok(recipes);
        }

        // POST v1/recipes
        [HttpPost("")]
        public async Task<IActionResult> PostNewRecipe([FromBody] NewRecipe newRecipe)
        {
            if(newRecipe == null)
            {
                _logger.LogWarning("Null posted body.");
                return BadRequest();
            }

            try
            {
                ValidateNewRecipe(newRecipe);
                var recipe = ConvertToRecipe(newRecipe);
                await _recipeRepository.InsertAsync(recipe).ConfigureAwait(false);
                return Created("https://example.com", recipe);
            }
            catch (ArgumentException e)
            {
                _logger.LogWarning("Semantically incorrect Recipe");
                _logger.LogWarning(e.Message);
                return BadRequest(e.Message);
            }
        }

        // GET v1/recipes/{recipeId}
        [HttpGet("{recipeId}")]
        public async Task<IActionResult> GetRecipeById([FromRoute] int recipeId)
        {
            if(recipeId == default(int))
            {
                _logger.LogWarning("Path parameter recipeId is the default value.");
                return BadRequest();
            }

            try
            {
                var recipe = await _recipeRepository.GetAsync(recipeId).ConfigureAwait(false);
                return Ok(recipe);
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(e.Message);
            }
        }

        [HttpPatch("{recipeId)")]
        public async Task<IActionResult> PatchRecipe([FromRoute] int recipeId, [FromBody] PatchRecipeBody patchRecipeBody)
        {
            if(recipeId == default(int))
            {
                _logger.LogWarning("Recipe Id path parameter is default");
                return BadRequest();
            }

            if(patchRecipeBody == null)
            {
                _logger.LogWarning("PatchRecipeBody is null.");
                return BadRequest();
            }

            _logger.LogInformation($"Patch for Recipe : {recipeId}");

            try
            {
                var recipe = await _recipeRepository.GetAsync(recipeId).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(patchRecipeBody.Name))
                {
                    recipe.Name = patchRecipeBody.Name;
                }
                if (!string.IsNullOrEmpty(patchRecipeBody.Instructions))
                {
                    recipe.Instructions = patchRecipeBody.Instructions;
                }
                if (patchRecipeBody.GlasswareId != default(int))
                {
                    recipe.Glassware = await _glasswareRepository.GetAsync(patchRecipeBody.GlasswareId);
                }

                await _recipeRepository.UpdateAsync(recipe).ConfigureAwait(false);

                return NoContent();
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(e.Message);
            }
        }

        internal static void ValidateNewRecipe(NewRecipe newRecipe)
        {
            if (string.IsNullOrEmpty(newRecipe.Name))
            {
                throw new ArgumentException("Recipe Name must not be empty");
            }

            if (newRecipe.Components.Any(c => c.ComponentId == default(int) || string.IsNullOrEmpty(c.QuantityPart)))
            {
                throw new ArgumentException("Invalid Recipe Component");
            }
        }

        internal static Recipe ConvertToRecipe(NewRecipe newRecipe)
        {
            return new Recipe
            {
                Name = newRecipe.Name,
                Instructions = newRecipe.Instructions,
                Glassware = new Glass { Id = newRecipe.GlasswareId },
                Components = newRecipe.Components.Select(c => new RecipeComponent
                {
                    ComponentId = c.ComponentId,
                    QuantityPart = c.QuantityPart,
                    QuantityImperial = c.QuantityImperial,
                    QuantityMetric = c.QuantityMetric
                }).ToList()
            };
        }
    }
}
