Feature: CreateBooking
In order to acommodation during my trip a room
As a customer
I want to be able to book a room

# Fully occupied period: [Today + 4; Today + 14]

@mytag
Scenario Outline: Create a booking
    Given I have entered a start date in <start> days
    And I have entered an end date in <end> days
    When I press Create New Booking
    Then The result should be <created>

    Examples:
    | start | end | created | 
    | 1     | 1   | true    |
    | 2     | 2   | true    |
    | 9     | 9   | true    |
    | 21    | 21  | true    |
    | 22    | 22  | true    |
    | 11    | 11  | false   |
    | 15    | 15  | false   |
    | 19    | 19  | false   |
