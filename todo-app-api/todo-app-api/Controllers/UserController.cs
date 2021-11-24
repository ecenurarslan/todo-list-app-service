using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_app_api.Dto;
using todo_app_api.Entity;
using todo_app_api.Service;

namespace todo_app_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_userService.Get());
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize]
        [HttpGet("CheckLogin")]
        public IActionResult CheckLogin()
        {
            return Ok();
        }
        [HttpPost("Login")]
        public IActionResult Login(UserDto.Login login)
        {
            try
            {
                return Ok(_userService.Login(login));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("Register")]
        public IActionResult Register(UserDto.Register register)
        {
            try
            {
                return Ok(_userService.Register(register));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
