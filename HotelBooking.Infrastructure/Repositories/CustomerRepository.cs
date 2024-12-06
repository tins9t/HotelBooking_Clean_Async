using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Core;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly HotelBookingContext db;

        public CustomerRepository(HotelBookingContext context)
        {
            db = context;
        }

        public async Task AddAsync(Customer entity)
        {
            throw new NotImplementedException();
        }

        public async Task EditAsync(Customer entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> GetAsync(int id)
        {
            return await db.Customer.FirstOrDefaultAsync(r => r.Id == id);

        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await db.Customer.ToListAsync();
        }

        public async Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
