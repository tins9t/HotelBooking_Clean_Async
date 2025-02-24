using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Moq;
using Xunit;

using HotelBooking.Core;
using HotelBooking.UnitTests.TestFixtures;

namespace HotelBooking.UnitTests;

[Collection("BookingManager collection")]
public class CreateBookingTests
{
    private BookingManagerFixture _fixture;

    public CreateBookingTests(BookingManagerFixture fixture)
    {
        _fixture = fixture;
        var bookings = new List<Booking>()
        {
            // Fully occupied period for Room 1 and Room 2
            new()
            {
                Id = 1,
                StartDate = DateTime.Today.AddDays(10),
                EndDate = DateTime.Today.AddDays(20),
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            },
            new()
            {
                Id = 2,
                StartDate = DateTime.Today.AddDays(10),
                EndDate = DateTime.Today.AddDays(20),
                IsActive = true,
                CustomerId = 2,
                RoomId = 2
            }
        };
        _fixture.mockBookingRepository.Setup(b => b.GetAllAsync()).ReturnsAsync(bookings);
    }

    /// <summary>
    /// Tests that a booking is accepted when no days are occupied for the desired period (for all rooms).
    /// </summary>
    /// <param name="numberOfDays1">The number of days from today to the start date of the booking.</param>
    /// <param name="numberOfDays2">The number of days from today to the end date of the booking.</param>
    /// <param name="expectedResult">The expected result of the booking creation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(1, 9, true)]
    [InlineData(21, 30, true)]
    public async Task CreateBooking_SuccessfulBooking_ReturnsTrue(int numberOfDays1, int numberOfDays2, bool expectedResult)
    {
        // Arrange
        var booking = new Booking
        {
            Id = 1,
            StartDate = DateTime.Today.AddDays(numberOfDays1),
            EndDate = DateTime.Today.AddDays(numberOfDays2),
            IsActive = true,
            CustomerId = 1,
            RoomId = 1
        };
        var bookingManager = _fixture.BookingManager;

        // Act
        var result = await bookingManager.CreateBooking(booking);

        // Assert
        Assert.Equal(result, expectedResult);
    }
    
    /// <summary>
    /// Tests that a booking is not accepted when the dates are invalid.
    /// </summary>
    /// <param name="numberOfDays1">The number of days from today to the start date of the booking.</param>
    /// <param name="numberOfDays2">The number of days from today to the end date of the booking.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(0, 0)]
    [InlineData(2, 1)]
    public async Task CreateBooking_UnsuccessfulBooking_ReturnsFalse(int numberOfDays1, int numberOfDays2)
    {
        // Arrange
        var booking = new Booking
        {
            Id = 1,
            StartDate = DateTime.Today.AddDays(numberOfDays1),
            EndDate = DateTime.Today.AddDays(numberOfDays2),
            IsActive = true,
            CustomerId = 1,
            RoomId = 1
        };
        var bookingManager = _fixture.BookingManager;
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => bookingManager.CreateBooking(booking));
    }

    /// <summary>
    /// Tests that a booking is not accepted when the start date is before the fully occupied period and
    /// the end date is after the fully occupied period.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task CreateBooking_StartDateBeforeOccupiedPeriodEndDateAfterOccupiedPeriod_ReturnsFalse()
    {
        // Arrange
        var booking = new Booking
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
        var result = await bookingManager.CreateBooking(booking);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests that a booking is not accepted when it overlaps with an existing booking.
    /// </summary>
    /// <param name="numberOfDays1">The number of days from today to the start date of the booking.</param>
    /// <param name="numberOfDays2">The number of days from today to the end date of the booking.</param>
    /// <param name="expectedResult">The expected result of the booking creation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Theory]
    [InlineData(9, 10, false)]
    [InlineData(9, 15, false)]
    [InlineData(9, 20, false)]
    [InlineData(10, 21, false)]
    [InlineData(15, 21, false)]
    [InlineData(20, 21, false)]
    public async Task CreateBooking_BookingOverlapsWithExistingBooking_ReturnsFalse(int numberOfDays1, int numberOfDays2, bool expectedResult)
    {
        // Arrange
        var booking = new Booking
        {
            Id = 1,
            StartDate = DateTime.Today.AddDays(numberOfDays1),
            EndDate = DateTime.Today.AddDays(numberOfDays2),
            IsActive = true,
            CustomerId = 1,
            RoomId = 1
        };
        var bookingManager = _fixture.BookingManager;

        // Act
        var result = await bookingManager.CreateBooking(booking);

        // Assert
        Assert.Equal(result, expectedResult);
    }
}