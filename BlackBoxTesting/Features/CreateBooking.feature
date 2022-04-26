Feature: CreateBooking
	In order to book a room
	As a booking manager
	I want to book an available room from a given start date to end date
@mytag
Scenario Outline: Create booking
	Given I as a Booking Manager have a list of bookings
	| StartDate | EndDate | IsActive | 
	| 1         | 2       | true     |
	| 9         | 21      | false    |
	| 21        | 22      | true     |
	| 9         | 11      | false    |
	| 11        | 19      | false    |
	| 19        | 21      | false    |
	When I press Create booking
	Then the booking is <IsActive>
