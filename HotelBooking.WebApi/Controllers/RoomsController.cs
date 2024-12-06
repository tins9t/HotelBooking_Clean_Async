using System;
using System.Collections.Generic;
using HotelBooking.Core;
using Microsoft.AspNetCore.Mvc;


namespace HotelBooking.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : Controller
    {
        private readonly IRepository<Room> repository;

        public RoomsController(IRepository<Room> repos)
        {
            repository = repos;
        }

        // GET: rooms
        [HttpGet(Name = "GetRooms")]
        public async Task<IEnumerable<Room>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET rooms/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await repository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST roooms
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Room room)
        {
            if (room == null)
            {
                return BadRequest();
            }

            await repository.AddAsync(room);
            return CreatedAtRoute("GetRooms", null);
        }


        // DELETE rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await repository.RemoveAsync(id);
                return NoContent();
            }
            else {
                return BadRequest();
            }
        }

    }
}
