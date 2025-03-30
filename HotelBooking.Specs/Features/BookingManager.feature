Feature: BookingManager

Tests of dates for create booking method

# Fully occupied periods where no rooms are available:
# From DateTime.Today.AddDays(10) to DateTime.Today.AddDays(20)

@validBooking
Scenario: Create booking with valid dates
	Given the first number is <startDays>
	And the second number is <endDays>
	When the booking is created
	Then the result should be True

Examples: 
  | startDays | endDays |
  | 1         | 9       | // SD: B, ED: B
  | 2         | 8       | // SD: B, ED: B
  | 21        | 30      | // SD: A, ED: A
  | 22        | 30      | // SD: A, ED: A


@occupiedDates
Scenario: Create booking with occupied dates
	Given the first number is <startDays>
	And the second number is <endDays>
	When the booking is created
	Then the result should be False

Examples: 
  | startDays | endDays |
  | 5         | 21      | // SD: B, ED: A
  | 5         | 10      | // SD: B, ED: O	
  | 10        | 21      | // SD: O, ED: A
  | 10        | 20      | // SD: O, ED: O
  | 11        | 19      | // SD: O, ED: O



@invalidDates
Scenario: Create booking with invalid dates
	Given the first number is <startDays>
	And the second number is <endDays>
	When trying to book, an error is thrown

Examples: 
  | startDays | endDays |
  | -5        | 10      | // SD: P, ED: -
  | 0         | 10      | // SD: T, ED: -
  | 21        | 5       | // SD: A, ED: B
  | 9         | 5       | // SD: B(later), ED: B(earlier)
  | 25        | 21      | // SD: A(later), ED: A(earlier)
  | 5         | 0       | // SD: B, ED: T
  | 5         | -5      | // SD: B, ED: P

