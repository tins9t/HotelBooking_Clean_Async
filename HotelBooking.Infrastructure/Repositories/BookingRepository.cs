using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Core;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class BookingRepository : IRepository<Booking>
    {
        private readonly HotelBookingContext db;

        public BookingRepository(HotelBookingContext context)
        {
            db = context;
        }

        public async Task AddAsync(Booking entity)
        {
            db.Booking.Add(entity);
            await db.SaveChangesAsync();
        }

        public async Task EditAsync(Booking entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<Booking> GetAsync(int id)
        {
            return await db.Booking.Include(b => b.Customer).
                Include(b => b.Room).
                FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await db.Booking.Include(b => b.Customer).
                Include(b => b.Room).
                ToListAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var booking = await db.Booking.FirstOrDefaultAsync(b => b.Id == id);
            db.Booking.Remove(booking);
            await db.SaveChangesAsync();
        }

    }
}
