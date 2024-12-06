using System.Collections.Generic;
using HotelBooking.Core;
using Microsoft.AspNetCore.Mvc;


namespace HotelBooking.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingsController : Controller
    {
        private IRepository<Booking> bookingRepository;
        private IBookingManager bookingManager;

        public BookingsController(IRepository<Booking> bookingRepos, IBookingManager manager)
        {
            bookingRepository = bookingRepos;
            bookingManager = manager;
        }

        // GET: bookings
        [HttpGet(Name = "GetBookings")]
        public async Task<IEnumerable<Booking>> Get()
        {
            return await bookingRepository.GetAllAsync();
        }

        // GET bookings/5
        [HttpGet("{id}", Name = "GetBooking")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await bookingRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST bookings
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Booking booking)
        {
            if (booking == null)
            {
                return BadRequest();
            }

            bool created = await bookingManager.CreateBooking(booking);

            if (created)
            {
                return CreatedAtRoute("GetBookings", null);
            }
            else
            {
                return Conflict("The booking could not be created. All rooms are occupied. Please try another period.");
            }

        }

        // PUT bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Booking booking)
        {
            if (booking == null || booking.Id != id)
            {
                return BadRequest();
            }

            var modifiedBooking = await bookingRepository.GetAsync(id);

            if (modifiedBooking == null)
            {
                return NotFound();
            }

            // This implementation will only modify the booking's state and customer.
            // It is not safe to directly modify StartDate, EndDate and Room, because
            // it could conflict with other active bookings.
            modifiedBooking.IsActive = booking.IsActive;
            modifiedBooking.CustomerId = booking.CustomerId;

            await bookingRepository.EditAsync(modifiedBooking);
            return NoContent();
        }

        // DELETE bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await bookingRepository.GetAsync(id) == null)
            {
                return NotFound();
            }

            await bookingRepository.RemoveAsync(id);
            return NoContent();
        }

    }
}
