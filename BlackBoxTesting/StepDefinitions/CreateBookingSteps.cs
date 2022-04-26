using BlackBoxTesting.Features;
using HotelBooking.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BlackBoxTesting.StepDefinitions
{
    [Binding]
    public class CreateBookingSteps
    {
        Booking? booking;
        bool IsActive;
        BookingManager bookingManager = new(new HotelBooking.UnitTests.Fakes.FakeBookingRepository(DateTime.Today, DateTime.Today.AddDays(30)), new HotelBooking.UnitTests.Fakes.FakeRoomRepository());

        [Given(@"I as a Booking Manager have a list of bookings")]
        public void GivenIAsABookingManagerHaveAListOfBookings(Table table)
        {
            this.booking = table.CreateInstance<HotelBooking.Core.Booking>();
        }

        [When(@"I press Create booking")]
        public void WhenIPressCreateBooking()
        {
            this.IsActive = this.bookingManager.CreateBooking(booking);
        }

        [Then(@"the booking is (.*)")]
        public void ThenTheBookingIs(bool isActive) 
        {
            Assert.Equals(this.IsActive, isActive);
        }
    }
}
