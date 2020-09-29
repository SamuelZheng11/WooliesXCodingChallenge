using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WooliesXCodingChallenge.Models;
using WooliesXCodingChallenge.Services;

namespace WooliesXCodingChallenge.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UserController(IUserService userService, ILogger logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            // It did not look like there were DB requirements so I've just returned a `user` (non-plural) which is myself and the token I was provided in the example
            // The service just sets up a Task that returns a straight user
            _logger.LogDebug(String.Format("Requested User {0}", await _userService.GetUser()));
            return await _userService.GetUser();
        }
    }
}
