using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core;

namespace HotelBooking.UnitTests.Fakes
{
    public class FakeBookingRepository : IRepository<Booking>
    {
        private DateTime fullyOccupiedStartDate;
        private DateTime fullyOccupiedEndDate;

        public FakeBookingRepository(DateTime start, DateTime end)
        {
            fullyOccupiedStartDate = start;
            fullyOccupiedEndDate = end;
        }

        // This field is exposed so that a unit test can validate that the
        // Add method was invoked.
        public bool addWasCalled = false;

        public Task AddAsync(Booking entity)
        {
            addWasCalled = true;
            return Task.CompletedTask;
        }

        // This field is exposed so that a unit test can validate that the
        // Edit method was invoked.
        public bool editWasCalled = false;

        public Task EditAsync(Booking entity)
        {
            editWasCalled = true;
            return Task.CompletedTask;
        }

        public Task<Booking> GetAsync(int id)
        {
            Task<Booking> bookingTask = Task.Factory.StartNew(() => new Booking
            {
                Id = 1, 
                StartDate = fullyOccupiedStartDate, 
                EndDate = fullyOccupiedEndDate, 
                IsActive = true, 
                CustomerId = 1, 
                RoomId = 1
            } );

            return bookingTask;
        }

        public Task<IEnumerable<Booking>> GetAllAsync()
        {
            IEnumerable<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=DateTime.Today.AddDays(1), EndDate=DateTime.Today.AddDays(1), IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=1, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=2, RoomId=2 },
            };
            
            Task<IEnumerable<Booking>> bookingsTask = Task.Factory.StartNew(() => bookings);
            
            return bookingsTask;
        }

        // This field is exposed so that a unit test can validate that the
        // Remove method was invoked.
        public bool removeWasCalled = false;

        public Task RemoveAsync(int id)
        {
            removeWasCalled = true;
            return Task.CompletedTask;
        }
    }
}
