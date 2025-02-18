using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core;
using HotelBooking.UnitTests.TestFixtures;
using Xunit;

namespace HotelBooking.UnitTests;

public class CreateBookingTests
{
        BookingManagerFixture _fixture;

        public CreateBookingTests(BookingManagerFixture fixture)
        {
            _fixture = fixture;
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
        
        // [Theory]
        // [InlineData("2025-02-01", "2025-03-28", 11)]
        // Note: Is it even possible to test this method with constant DateTime values because
        // the method checks if the start date is in the past? The test would stop working at some point in the future.
        
        [Fact]
        public async Task GetFullyOccupiedDates_TestDataHasElevenFullyOccupiedDates_ReturnsAllDates()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today.AddDays(30);
            // DateTime startDate = DateTime.Parse(startDateString);
            // DateTime endDate = DateTime.Parse(endDateString);
            var bookingManager = _fixture.BookingManager;
            
            // Act
            List<DateTime> fullyOccupiedDates = await bookingManager.GetFullyOccupiedDates(startDate, endDate);
            
            // Assert
            Assert.Equal(11, fullyOccupiedDates.Count);
        }
}