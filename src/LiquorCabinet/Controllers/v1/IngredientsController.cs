    using LiquorCabinet.Models;
using LiquorCabinet.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquorCabinet.Controllers.v1
{
    [Route("v1/ingredients")]
    public class IngredientsController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICrudRepository<int, Ingredient> _ingredientRepository;

        public IngredientsController(ILogger<IngredientsController> logger, ICrudRepository<int, Ingredient> repository)
        {
            _logger = logger;
            _ingredientRepository = repository;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateIngredient([FromBody] Ingredient postedIngredient)
        {
            if(postedIngredient == null)
            {
                _logger.LogWarning("Malformed body.");
                return BadRequest();
            }

            await _ingredientRepository.InsertAsync(postedIngredient).ConfigureAwait(false);

            return Created("https://example.com", postedIngredient);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _ingredientRepository.GetListAsync().ConfigureAwait(false);
            return Ok(ingredients);
        }

        [HttpGet("{ingredientId}")]
        public async Task<IActionResult> GetIngredient([FromRoute] int ingredientId)
        {
            if(ingredientId == default(int))
            {
                _logger.LogWarning("Default IngredientId path parameter!");
                return BadRequest();
            }
            try
            {
                var ingredient = await _ingredientRepository.GetAsync(ingredientId).ConfigureAwait(false);
                return Ok(ingredient);
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(e.Message);
            } 
        }

        [HttpPut("{ingredientId}")]
        public async Task<IActionResult> UpdateIngredient([FromRoute] int ingredientId, [FromBody] Ingredient ingredient)
        {
            if(ingredientId == default(int))
            {
                _logger.LogWarning("Default IngredientId path parameter!");
                return BadRequest();
            }
            if(ingredient == null)
            {
                _logger.LogWarning("Malformed body");
                return BadRequest();
            }
            if(ingredientId != ingredient.Id)
            {
                _logger.LogWarning("Path paramater and body id mismatch.");
                return BadRequest();
            }

            try
            {
                await _ingredientRepository.UpdateAsync(ingredient).ConfigureAwait(false);
                return NoContent();
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{ingredientId}")]
        public async Task<IActionResult> DeleteIngredient([FromRoute] int ingredientId)
        {
            if(ingredientId == default(int))
            {
                _logger.LogWarning("Default IngredientId path parameter!");
            }

            try
            {
                await _ingredientRepository.DeleteAsync(ingredientId).ConfigureAwait(false);
                return Ok();
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound();
            }
        }
    }
}
