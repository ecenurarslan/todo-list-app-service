using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using todo_app_api.Dto;
using todo_app_api.Entity;
using todo_app_api.Service;
using System.Drawing;
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
        public async Task<IActionResult> Register(IList<IFormFile> files,[FromForm] string user)
        {
            IFormFile formFile = files[0];
            try
            {
                string uploads = Path.Combine("uploads");
       
                    if (formFile.Length > 0)
                    {
                        string filePath = Path.Combine(uploads, formFile.FileName + ".png"); 
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(fileStream);
                        }
                    }
                UserDto.Register userData= JsonSerializer.Deserialize<UserDto.Register>(user);
                return Ok(_userService.Register(userData));
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
    }
}
