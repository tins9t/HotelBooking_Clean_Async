using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;

namespace HotelBooking.UnitTests.TestFixtures;

public class BookingManagerFixture : IDisposable
{
    public IBookingManager BookingManager { get; }
    public Mock<IRepository<Booking>> mockBookingRepository;
    public Mock<IRepository<Room>> mockRoomRepository;
    
    public BookingManagerFixture()
    {
        mockBookingRepository = new Mock<IRepository<Booking>>();
        mockRoomRepository = new Mock<IRepository<Room>>();
        
        BookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);
        
        // Set up mock data for rooms
        List<Room> rooms = new List<Room>()
        {
            new Room { Id = 1, Description = "Room 1" },
            new Room { Id = 2, Description = "Room 2" }
        };
        
        // Set up mock methods for the mock room repository
        // GetAllAsync
        mockRoomRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(rooms);
        
        // GetAsync
        mockRoomRepository.Setup(repo => repo.GetAsync(1)).ReturnsAsync(rooms[0]);
        // Alternative setup with argument matchers:
        // Any integer:
        // mockRoomRepository.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(rooms[1]);
        // Integers from 1 to 2 (using a predicate)
        //mockRoomRepository.Setup(repo => repo.GetAsync(It.Is<int>(id => id > 0 && id < 3))).ReturnsAsync(rooms[1]);
        // Integers from 1 to 2 (using a range)
        // mockRoomRepository.Setup(repo => repo.GetAsync(It.IsInRange<int>(1, 2, Moq.Range.Inclusive))).ReturnsAsync(rooms[1]);
    }
    
    public void Dispose()
    {
        // There is nothing to dispose
    }
}