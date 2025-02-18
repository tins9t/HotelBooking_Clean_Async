using System;
using System.Threading.Tasks;
using HotelBooking.UnitTests.TestData;
using HotelBooking.UnitTests.TestFixtures;
using Xunit;

namespace HotelBooking.UnitTests;

[Collection("BookingManager collection")]
public class ValidateDatesTests
{
    BookingManagerFixture _fixture;
    
    public ValidateDatesTests(BookingManagerFixture fixture)
    {
        _fixture = fixture;
    }
    /// <summary>
    /// Tests for valid dates
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    [Theory]
    [MemberData(nameof(TestDatesData.ValidFutureDates),  MemberType= typeof(TestDatesData))]
    public async Task FindAvailableRooms_BookingDatesInFuture_ShouldReturnRoomIdOrMinusOne(DateTime startDate, DateTime endDate)
    {
        //Arrange
        var bookingManager = _fixture.BookingManager;
        
        // Act

        var roomId = await bookingManager.FindAvailableRoom(startDate, endDate);
        
        // Assert
        Assert.True(roomId == -1 || roomId > 0); 
    }
    
    /// <summary>
    /// Test for invalid dates
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    [Theory]
    [MemberData(nameof(TestDatesData.InvalidFutureDates),  MemberType= typeof(TestDatesData))]
    public async Task FindAvailableRooms_BookingDatesNotInFuture_ShouldThrowException(DateTime startDate, DateTime endDate)
    {
        // Arrange
        var bookingManager = _fixture.BookingManager;

        // Act
        Task result() => bookingManager.FindAvailableRoom(startDate, endDate);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
    }
}