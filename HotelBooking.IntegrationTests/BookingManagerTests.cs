using System;
using System.Threading.Tasks;
using HotelBooking.Core;
using HotelBooking.Infrastructure;
using HotelBooking.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HotelBooking.IntegrationTests
{
    public class BookingManagerTests : IDisposable
    {
        // This test class uses a separate Sqlite in-memory database. While the
        // .NET Core built-in in-memory database is not a relational database,
        // Sqlite in-memory database is. This means that an exception is thrown,
        // if a database constraint is violated, and this is a desirable behavior
        // when testing.

        SqliteConnection connection;
        BookingManager bookingManager;

        public BookingManagerTests()
        {
            connection = new SqliteConnection("DataSource=:memory:");

            // In-memory database only exists while the connection is open
            connection.Open();

            // Initialize test database
            var options = new DbContextOptionsBuilder<HotelBookingContext>()
                            .UseSqlite(connection).Options;
            var dbContext = new HotelBookingContext(options);
            IDbInitializer dbInitializer = new DbInitializer();
            dbInitializer.Initialize(dbContext);

            // Create repositories and BookingManager
            var bookingRepos = new BookingRepository(dbContext);
            var roomRepos = new RoomRepository(dbContext);
            bookingManager = new BookingManager(bookingRepos, roomRepos);
        }

        public void Dispose()
        {
            // This will delete the in-memory database
            connection.Close();
        }

        [Fact]
        public async Task FindAvailableRoom_RoomNotAvailable_RoomIdIsMinusOne()
        {
            // Act
            var roomId = 
                await bookingManager.FindAvailableRoom(DateTime.Today.AddDays(8), DateTime.Today.AddDays(8));
            // Assert
            Assert.Equal(-1, roomId);
        }
    }
}
