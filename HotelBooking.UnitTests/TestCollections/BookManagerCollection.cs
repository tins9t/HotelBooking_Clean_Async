using HotelBooking.UnitTests.TestFixtures;
using Xunit;

namespace HotelBooking.UnitTests.TestCollections;

[CollectionDefinition("BookingManager collection")]
public class BookManagerCollection : ICollectionFixture<BookingManagerFixture>
{
    
}