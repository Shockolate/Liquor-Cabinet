using LiquorCabinet.Repositories;
using LiquorCabinet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquorCabinet.Controllers.v1
{
    [Route("v1/glassware")]
    public class GlasswareController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICrudRepository<int, Glass> _glasswareRepository;

        public GlasswareController(ILogger<GlasswareController> logger, ICrudRepository<int, Glass> glasswareRepository)
        {
            _logger = logger;
            _glasswareRepository = glasswareRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllGlassware()
        {
            var glasswareList = await _glasswareRepository.GetListAsync().ConfigureAwait(false);
            return Ok(glasswareList);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateNewGlass([FromBody] Glass postedGlass)
        {
            if(postedGlass == null)
            {
                _logger.LogWarning("posted glass is null.");
                return BadRequest();
            }

            try
            {
                await _glasswareRepository.InsertAsync(postedGlass).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Inserting New Glass: {e.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Created("https://example.com", postedGlass);
        }
        
        [HttpGet("{glasswareId}")]
        public async Task<IActionResult> GetGlass([FromRoute] int glasswareId)
        {
            if(glasswareId == default(int))
            {
                _logger.LogWarning("Default value found in GlasswareId");
                return BadRequest();
            }
            try
            {
                var glass = await _glasswareRepository.GetAsync(glasswareId).ConfigureAwait(false);
                return Ok(glass);
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogWarning(e.Message);
                return NotFound(e.Message);
            }
            
        }
    }
}
