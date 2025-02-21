using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core;
using HotelBooking.UnitTests.TestFixtures;
using Xunit;
using Moq;

namespace HotelBooking.UnitTests;

[Collection("BookingManager collection")]
public class CreateBookingTests
{
        BookingManagerFixture _fixture;

        public CreateBookingTests(BookingManagerFixture fixture)
        {
            _fixture = fixture;
             List<Booking> bookings = new List<Booking>()
                    {
                        // Fully occupied period for Room 1 and Room 2
                        new Booking
                        {
                            Id = 1,
                            StartDate = DateTime.Today.AddDays(10),
                            EndDate = DateTime.Today.AddDays(20),
                            IsActive = true,
                            CustomerId = 1,
                            RoomId = 1
                        },
                        new Booking
                        {
                            Id = 2,
                            StartDate = DateTime.Today.AddDays(10),
                            EndDate = DateTime.Today.AddDays(20),
                            IsActive = true,
                            CustomerId = 2,
                            RoomId = 2
                        },
                    };
             
             _fixture.mockBookingRepository.Setup(b => b.GetAllAsync()).ReturnsAsync(bookings);
                    
        }

        // Test: Booking is accepted when no days are occupied for the desired period (for all rooms).
        [Fact]
        public async Task CreateBooking_SuccessfulBooking_ReturnsTrue()
        {
            // Arrange
            Booking booking = new Booking
            {
                Id = 1,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(1),
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };
            var bookingManager = _fixture.BookingManager;

            // Act
            bool result = await bookingManager.CreateBooking(booking);

            // Assert
            Assert.True(result);
        }

        // A test with a start date before the fully occupied period and an end date after the fully occupied period.
        [Fact]
        public async Task CreateBooking_StartDateBeforeOccupiedPeriodEndDateAfterOccupiedPeriod_ReturnsFalse()
        {
            // Arrange
            Booking booking = new Booking
            {
                Id = 1,
                StartDate = DateTime.Today.AddDays(9),
                EndDate = DateTime.Today.AddDays(21),
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };
            var bookingManager = _fixture.BookingManager;

            // Act
            bool result = await bookingManager.CreateBooking(booking);

            // Assert
            Assert.False(result);
        }
        
        [Fact]
        public async Task CreateBooking_BookingOverlapsWithExistingBooking_ReturnsFalse()
        {
            // Arrange
            Booking booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(9),
                EndDate = DateTime.Today.AddDays(15),
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };
            var bookingManager = _fixture.BookingManager;
        
            // Act
            bool result = await bookingManager.CreateBooking(booking);
        
            // Assert
            Assert.False(result);
        }
        
}