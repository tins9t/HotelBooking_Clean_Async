using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reqnroll;
using Moq;

using HotelBooking.Core;
using HotelBooking.UnitTests.TestFixtures;
using Xunit;

namespace HotelBooking.Specs.StepDefinitions;

[Binding]
public sealed class BookingManagerStepDefinitions
{
    private BookingManagerFixture _fixture;
    private int _startDate;
    private int _endDate;
    private bool _result;
    private Booking booking;
    
    public BookingManagerStepDefinitions(BookingManagerFixture fixture)
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

    [Given(@"the first number is (.*)")]
    public void GivenTheFirstNumberIs(int startDays)
    {
        _startDate = startDays;
    }

    [Given(@"the second number is (.*)")]
    public void GivenTheSecondNumberIs(int endDays)
    {
        _endDate = endDays;

        booking = new Booking
        {
            Id = 1,
            StartDate = DateTime.Today.AddDays(_startDate),
            EndDate = DateTime.Today.AddDays(_endDate),
            IsActive = true,
            CustomerId = 1,
            RoomId = 1
        };
    }

    [When(@"the booking is created")]
    public async Task WhenTheBookingIsCreated()
    {
        _result = await _fixture.BookingManager.CreateBooking(booking);
    }
    
    [When(@"trying to book, an error is thrown")]
    public async Task WhenTryingToBookAnErrorIsThrown()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _fixture.BookingManager.CreateBooking(booking));
    }

    [Then(@"the result should be (.*)")]
    public void ThenTheResultShouldBe(Boolean result)
    {
        Assert.Equal(result, _result);
    }
}