using LiquorCabinet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquorCabinet.Controllers.v1
{
    [Route("v1/users")]
    public class UsersController : Controller
    {
        private readonly ILogger _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpPost("")]
        public Task<IActionResult> CreateUser([FromBody] User user)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{userId}")]
        public Task<IActionResult> GetUser([FromRoute] int userId)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{userId}")]
        public Task<IActionResult> UpdateUser([FromRoute] int userId, [FromBody] User updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
