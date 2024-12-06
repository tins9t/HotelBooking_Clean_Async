using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Core;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class RoomRepository : IRepository<Room>
    {
        private readonly HotelBookingContext db;

        public RoomRepository(HotelBookingContext context)
        {
            db = context;
        }

        public async Task AddAsync(Room entity)
        {
            db.Room.Add(entity);
            
            await db.SaveChangesAsync();
        }

        public async Task EditAsync(Room entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Room> GetAsync(int id)
        {
            // The FirstOrDefault method below returns null
            // if there is no room with the specified Id.
            return await db.Room.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await db.Room.ToListAsync();
        }

        public async Task RemoveAsync(int id)
        {
            // The Single method below throws an InvalidOperationException
            // if there is not exactly one room with the specified Id.
            var room = await db.Room.SingleAsync(r => r.Id == id);
            db.Room.Remove(room);
            await db.SaveChangesAsync();
        }
    }
}
