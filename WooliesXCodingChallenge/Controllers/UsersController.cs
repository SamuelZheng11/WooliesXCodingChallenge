using System;
using System.Collections.Generic;
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
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public User GetUser()
        {
            return new User() {
                Name = "Samuel Zheng",
                Token = "1234-455662-22233333-3333"
            };
        }
    }
}
