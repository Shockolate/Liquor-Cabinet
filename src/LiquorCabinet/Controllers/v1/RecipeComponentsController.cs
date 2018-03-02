using LiquorCabinet.Models;
using LiquorCabinet.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using LiquorCabinet.Repositories;
using System.Linq;
using System.Threading.Tasks;


namespace LiquorCabinet.Controllers.v1
{
    [Route("v1/recipes/{recipeId}/components")]
    public class RecipeComponentsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICrudRepository<int, RecipeComponent> _recipeComponentRepository;

        public RecipeComponentsController(ILogger<RecipeComponentsController> logger, IRecipeRepository recipeRepository, ICrudRepository<int, RecipeComponent> recipeComponentRepository)
        {
            _logger = logger;
            _recipeRepository = recipeRepository;
            _recipeComponentRepository = recipeComponentRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> PostComponentToRecipe([FromRoute] int recipeId, [FromBody] RecipeComponent recipeComponent)
        {
            if(recipeId == default(int))
            {
                _logger.LogWarning("recipeId path parameter is default.");
                return BadRequest();
            }

            if(recipeComponent == null)
            {
                _logger.LogWarning("RecipeComponent body is null.");
                return BadRequest();
            }

            if(recipeComponent.RecipeId != recipeId)
            {
                var errorMessage = $"RecipeId in the path ({recipeId}) must match the RecipeId in the body ({recipeComponent.RecipeId}).";
                _logger.LogWarning(errorMessage);
                return BadRequest(errorMessage);
            }

            try
            {
                await _recipeRepository.AddComponentsToRecipeAsync(recipeId, new List<RecipeComponent>(1) { recipeComponent });
                return Created("https://example.com", recipeComponent);
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(e.Message);
            }
        }

        [HttpPatch("{componentId}")]
        public async Task<IActionResult> UpdateRecipeComponent([FromRoute] int recipeId, [FromRoute] int componentId, [FromBody] PatchComponentBody body)
        {
            if(recipeId == default(int) || componentId == default(int))
            {
                _logger.LogWarning("path parameters are default value.");
                return BadRequest();
            }

            if(body == null)
            {
                _logger.LogWarning("PatchComponentBody was null");
                return BadRequest();
            }

            _logger.LogInformation($"Updating Component {componentId} for recipe {recipeId}");

            try
            {
                var componentToUpdate = await _recipeComponentRepository.GetAsync(componentId).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(body.QuantityPart))
                {
                    componentToUpdate.QuantityPart = body.QuantityPart;
                }
                if (body.QuantityMetric.HasValue)
                {
                    componentToUpdate.QuantityMetric = body.QuantityMetric;
                }
                if (body.QuantityImperial.HasValue)
                {
                    componentToUpdate.QuantityImperial = body.QuantityImperial;
                }

                await _recipeComponentRepository.UpdateAsync(componentToUpdate).ConfigureAwait(false);

                return NoContent();
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{componentId}")]
        public async Task<IActionResult> DeleteRecipeComponent([FromQuery] int recipeId, [FromQuery] int componentId)
        {
            if(recipeId == default(int) || componentId == default(int))
            {
                _logger.LogWarning("Path Parameters are default values.");
                return BadRequest();
            }

            try
            {
                await _recipeComponentRepository.DeleteAsync(componentId).ConfigureAwait(false);
                return NoContent();
            } catch (EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(e.Message);
            }
        }
    }
}
