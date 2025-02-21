using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using HotelBooking.Core;
using HotelBooking.UnitTests.TestFixtures;

namespace HotelBooking.UnitTests
{
    [Collection("BookingManager collection")]
    public class CheckOccupiedDatesTest
    {
        BookingManagerFixture _fixture;

        public CheckOccupiedDatesTest(BookingManagerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetFullyOccupiedDates_WhenStartDateGreaterThanEndDate_ShouldThrowException()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(10);
            var endDate = DateTime.Now.AddDays(5); // Start date is after end date
            
            var bookingManager = _fixture.BookingManager;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => bookingManager.GetFullyOccupiedDates(startDate, endDate));
        }

        [Fact]
        public async Task GetFullyOccupiedDates_WhenNoBookings_ShouldReturnEmptyList()
        {
            // Arrange
            _fixture.mockBookingRepository.Setup(b => b.GetAllAsync()).ReturnsAsync(new List<Booking>());

            var startDate = DateTime.Now.AddDays(30);
            var endDate = DateTime.Now.AddDays(35);
            
            var bookingManager = _fixture.BookingManager;

            // Act
            var result = await bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFullyOccupiedDates_WhenAllRoomsBooked_ShouldReturnAllDatesInRange()
        {
            // Arrange
            var bookingStart = DateTime.Now.AddDays(40);
            var bookingEnd = DateTime.Now.AddDays(43);
            
            var bookings = new List<Booking>
            {
                new Booking { StartDate = bookingStart, EndDate = bookingEnd, IsActive = true, RoomId = 1 },
                new Booking { StartDate = bookingStart, EndDate = bookingEnd, IsActive = true, RoomId = 2 }
            };
            
            var bookingManager = _fixture.BookingManager;
            _fixture.mockBookingRepository.Setup(b => b.GetAllAsync()).ReturnsAsync(bookings);

            // Act
            var result = await bookingManager.GetFullyOccupiedDates(bookingStart, bookingEnd);

            // Assert
            Assert.Equal((bookingEnd - bookingStart).Days + 1, result.Count);
            foreach (var date in Enumerable.Range(0, (bookingEnd - bookingStart).Days + 1).Select(i => bookingStart.AddDays(i)))
            {
                Assert.Contains(date, result);
            }
        }

        [Fact]
        public async Task GetFullyOccupiedDates_WhenSomeDaysAreFullyBooked_ShouldReturnOnlyThoseDays()
        {
            // Arrange
            var bookingStart = DateTime.Now.AddDays(50);
            var bookingEnd = DateTime.Now.AddDays(55);

            // Fully booked on these specific days
            var fullyBookedDays = new List<DateTime>
            {
                bookingStart.AddDays(2), // Day 52
                bookingStart.AddDays(3)  // Day 53
            };
            
            var bookings = new List<Booking>
            {
                new Booking { StartDate = fullyBookedDays[0], EndDate = fullyBookedDays[0], IsActive = true, RoomId = 1 },
                new Booking { StartDate = fullyBookedDays[0], EndDate = fullyBookedDays[0], IsActive = true, RoomId = 2 },
                new Booking { StartDate = fullyBookedDays[1], EndDate = fullyBookedDays[1], IsActive = true, RoomId = 1 },
                new Booking { StartDate = fullyBookedDays[1], EndDate = fullyBookedDays[1], IsActive = true, RoomId = 2 }
            };
            
            var bookingManager = _fixture.BookingManager;
            _fixture.mockBookingRepository.Setup(b => b.GetAllAsync()).ReturnsAsync(bookings);

            // Act
            var result = await bookingManager.GetFullyOccupiedDates(bookingStart, bookingEnd);

            // Assert
            Assert.Equal(fullyBookedDays.Count, result.Count); // Expect 2 fully booked days
            foreach (var date in fullyBookedDays)
            {
                Assert.Contains(date, result);
            }
        }
    }
}
