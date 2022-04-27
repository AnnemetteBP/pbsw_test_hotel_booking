using BlackBoxTesting.Features;
using HotelBooking.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Moq;
using TechTalk.SpecFlow;

namespace BlackBoxTesting.Step
{
    [Binding]
    public class CreateBookingStepDefinitions
    {
        private IBookingManager bookingManager;
        private Mock<IRepository<Booking>> fakeBookingRepository;
        private Mock<IRepository<Room>> fakeRoomRepository;

        List<Booking> bookings = new List<Booking>
        {
            new Booking { Id=1, StartDate=DateTime.Today.AddDays(10), EndDate=DateTime.Today.AddDays(20), IsActive=true, CustomerId=1, RoomId=1 },
            new Booking { Id=2, StartDate=DateTime.Today.AddDays(10), EndDate=DateTime.Today.AddDays(20), IsActive=true, CustomerId=2, RoomId=2 }
        };

        List<Room> rooms = new List<Room>
        {
            new Room { Id=1, Description="A" },
            new Room { Id=2, Description="B" },
        };

        bool IsActive;
        DateTime start;
        DateTime end;

        [Given(@"I have entered a start date in (.*) days")]
        public void GivenIAsABookingManagerHaveAListOfBookings(int startDate)
        {
            start = DateTime.Today.AddDays(startDate);
        }

        [Given(@"I have entered an end date in (.*) days")]
        public void GivenIHaveEnteredAnEndDateInDays(int endDate)
        {
            end = DateTime.Today.AddDays(endDate);
        }

        [When(@"I press Create New Booking")]
        public void WhenIPressCreateBooking()
        {
            fakeBookingRepository = new Mock<IRepository<Booking>>();
            fakeRoomRepository = new Mock<IRepository<Room>>();

            fakeBookingRepository.Setup(x => x.GetAll()).Returns(bookings);
            fakeRoomRepository.Setup(x => x.GetAll()).Returns(rooms);

            bookingManager = new BookingManager(fakeBookingRepository.Object, fakeRoomRepository.Object);

            this.IsActive = bookingManager.CreateBooking(new Booking { Id = 1, StartDate = start, EndDate = end, IsActive = true, CustomerId = 1, RoomId = 1 });
        }

        [Then(@"The result should be (.*)")]
        public void ThenTheBookingIs(bool isActive)
        {
            Assert.AreEqual(this.IsActive, isActive);
        }
    }
}
