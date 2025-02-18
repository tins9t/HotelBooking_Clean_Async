using System;
using System.Collections.Generic;

namespace HotelBooking.UnitTests.TestData;

public class TestDatesData 
{
    public static IEnumerable<object[]> ValidFutureDates()
    {
        yield return new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(2) };  
        yield return new object[] { DateTime.Today.AddDays(4), DateTime.Today.AddDays(14) };
        yield return new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddMonths(3) }; 
        yield return new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddMonths(5) }; 
    }
    
    public static IEnumerable<object[]> InvalidFutureDates()
    {
        // End date comes before start date
        yield return new object[] { DateTime.Today.AddDays(5), DateTime.Today.AddDays(1) }; 
        yield return new object[] { DateTime.Today.AddDays(10), DateTime.Today.AddDays(5) }; 
        
        // End date is in the past
        yield return new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(-1) }; 
        yield return new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(-5) }; 
        yield return new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(-10) }; 
        

        // Start date is in the past
        yield return new object[] { DateTime.Today.AddMonths(-1), DateTime.Today.AddDays(1) }; 
        yield return new object[] { DateTime.Today.AddDays(-2), DateTime.Today.AddDays(2) };  
        yield return new object[] { DateTime.Today.AddMonths(-10), DateTime.Today.AddDays(10) }; 
    }
    

    public static IEnumerable<object[]> ExceedsMaxBookingDuration()
    {
        // In order to add this, we need to add this logic to the main method itself
        yield return new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddMonths(3) }; 
        yield return new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddMonths(3).AddDays(1) }; 
    }
}