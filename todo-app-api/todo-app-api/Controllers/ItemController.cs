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
    [Authorize(Roles ="Admin")]
    public class ItemController : ControllerBase
    {
        IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_itemService.Get(User.Identity.Name));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("Add")]
        public IActionResult Add(ItemDto.Add item)
        {
            try
            {
                int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                return Ok(_itemService.Add(item, userId));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("Update")]
        public IActionResult Update(ItemDto.Update item)
        {
            try
            {
                return Ok(_itemService.Update(item));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("Delete")]
        public IActionResult Delete([FromBody] int id)
        {
            try
            {
                return Ok(_itemService.Delete(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("List")]
        public IActionResult List()
        {
            try
            {
                int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                return Ok(_itemService.List(userId));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
