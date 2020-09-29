using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WooliesXCodingChallenge.Models;
using WooliesXCodingChallenge.Services;

namespace WooliesXCodingChallenge.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public User GetUser()
        {
            // It did not look like there were DB requirements so I've just returned a `user` (non-plural) which is myself and the token I was provided in the example
            return new User() {
                Name = "Samuel Zheng",
                Token = "1234-455662-22233333-3333"
            };
        }
    }
}
