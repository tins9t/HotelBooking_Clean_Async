﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using HotelBooking.Core;

namespace HotelBooking.UnitTests.TestFixtures
{
    [Collection("BookingManager collection")]
    public class CheckOccupiedDatesTest
    {
        private readonly Mock<IRepository<Room>> _roomRepositoryMock;
        private readonly Mock<IRepository<Booking>> _bookingRepositoryMock;
        private readonly IBookingManager _bookingManager;

        public CheckOccupiedDatesTest()
        {
            _roomRepositoryMock = new Mock<IRepository<Room>>();
            _bookingRepositoryMock = new Mock<IRepository<Booking>>();
            _bookingManager = new BookingManager(_bookingRepositoryMock.Object, _roomRepositoryMock.Object);
        }

        [Fact]
        public async Task GetFullyOccupiedDates_WhenStartDateGreaterThanEndDate_ShouldThrowException()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(10);
            var endDate = DateTime.Now.AddDays(5); // Start date is after end date

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _bookingManager.GetFullyOccupiedDates(startDate, endDate));
        }

        [Fact]
        public async Task GetFullyOccupiedDates_WhenNoBookings_ShouldReturnEmptyList()
        {
            // Arrange
            _roomRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Room> { new Room(), new Room() });
            _bookingRepositoryMock.Setup(b => b.GetAllAsync()).ReturnsAsync(new List<Booking>());

            var startDate = DateTime.Now.AddDays(30);
            var endDate = DateTime.Now.AddDays(35);

            // Act
            var result = await _bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFullyOccupiedDates_WhenAllRoomsBooked_ShouldReturnAllDatesInRange()
        {
            // Arrange
            var rooms = new List<Room> { new Room { Id = 1 }, new Room { Id = 2 } };
            var bookingStart = DateTime.Now.AddDays(40);
            var bookingEnd = DateTime.Now.AddDays(43);

            var bookings = new List<Booking>
            {
                new Booking { StartDate = bookingStart, EndDate = bookingEnd, IsActive = true, RoomId = 1 },
                new Booking { StartDate = bookingStart, EndDate = bookingEnd, IsActive = true, RoomId = 2 }
            };

            _roomRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(rooms);
            _bookingRepositoryMock.Setup(b => b.GetAllAsync()).ReturnsAsync(bookings);

            // Act
            var result = await _bookingManager.GetFullyOccupiedDates(bookingStart, bookingEnd);

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
            var rooms = new List<Room> { new Room { Id = 1 }, new Room { Id = 2 } };
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

            _roomRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(rooms);
            _bookingRepositoryMock.Setup(b => b.GetAllAsync()).ReturnsAsync(bookings);

            // Act
            var result = await _bookingManager.GetFullyOccupiedDates(bookingStart, bookingEnd);

            // Assert
            Assert.Equal(fullyBookedDays.Count, result.Count); // Expect 2 fully booked days
            foreach (var date in fullyBookedDays)
            {
                Assert.Contains(date, result);
            }
        }
    }
}
