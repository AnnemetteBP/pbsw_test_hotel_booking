using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;
        private Mock<IRepository<Booking>> fakeBookingRepository;
        private Mock<IRepository<Room>> fakeRoomRepository;

        public BookingManagerTests()
        {
            var fullyOccupiedStartDate = DateTime.Today.AddDays(10);
            var fullyOccupiedEndDate = DateTime.Today.AddDays(20);

            //DateTime start = DateTime.Today.AddDays(10);
            //DateTime end = DateTime.Today.AddDays(20);
            //IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            //IRepository<Room> roomRepository = new FakeRoomRepository();
            //bookingManager = new BookingManager(bookingRepository, roomRepository);

            //Moq needed lists
            var bookings = new List<Booking>
                {
                    new Booking { Id=1, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=1, RoomId=1 },
                    new Booking { Id=2, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=2, RoomId=2 }
                };

            var rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            // Create fake BookingRepository. 
            fakeBookingRepository = new Mock<IRepository<Booking>>();
            fakeRoomRepository = new Mock<IRepository<Room>>();

            // Implement fake GetAll() method.
            fakeBookingRepository.Setup(x => x.GetAll()).Returns(bookings);
            fakeRoomRepository.Setup(x => x.GetAll()).Returns(rooms);

            // Create RoomsController
            bookingManager = new BookingManager(fakeBookingRepository.Object, fakeRoomRepository.Object);
        }
        public static IEnumerable<object[]> GetLocalData()
        {
            var data = new List<object[]>
            {
                // User Story 1 - Case 3
                new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 1},
                // User Story 1 - Case 4
                new object[] { DateTime.Today.AddDays(9), DateTime.Today.AddDays(21), -1},
                // User Story 1 - Case 5
                new object[] { DateTime.Today.AddDays(21), DateTime.Today.AddDays(22), 1},
                // User Story 1 - Case 6
                new object[] { DateTime.Today.AddDays(9), DateTime.Today.AddDays(11), -1},
                // User Story 1 - Case 7
                new object[] { DateTime.Today.AddDays(11), DateTime.Today.AddDays(19), -1},
                // User Story 1 - Case 8
                new object[] { DateTime.Today.AddDays(19), DateTime.Today.AddDays(21), -1}
            };
            return data;
        }

        [Theory]
        [MemberData(nameof(GetLocalData))]
        public void FindAvailableRoom_ValidMemberData_RoomIdIsCorrect(DateTime startDate, DateTime endDate, int expectedRoomId)
        {
            //Arrange
            bookingManager = new BookingManager(fakeBookingRepository.Object, fakeRoomRepository.Object);
            //Act
            int RoomID = bookingManager.FindAvailableRoom(startDate, endDate);
            //Assert
            Assert.Equal(expectedRoomId, RoomID);
        }

        // Unit tests
        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            // Arrange
            DateTime date = DateTime.Today;

            // Act
            Action act = () => bookingManager.FindAvailableRoom(date, date);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        // User Story 1 - Case 1
        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFutureAndEndDateNotInTheFuture_ThrowsAurgumentException()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(-2);
            DateTime endDate = DateTime.Today.AddDays(-1);
            // Act
            Action act = () => bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        // User Story 1 - Case 2
        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFutureAndEndDateInTheFuture_ThrowsArgumentExeption()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today.AddDays(1);
            // Act
            Action act = () => bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Throws<ArgumentException>(act);
        }
        // User Story 1 - Case 3
        [Fact]
        public void FindAvailableRoom_StartDateInTheFutureAndEndDateInTheFuture_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(2);
            // Act
            int RoomID = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.NotEqual(-1, RoomID);
        }

        // User Story 1 - Case 4
        [Fact]
        public void FindAvailableRoom_StartDateBeforeFullyOccupiedAndEndDateAfterFullyOccupied_RoomIdMinusOne()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(9);
            DateTime endDate = DateTime.Today.AddDays(21);
            // Act
            int RoomID = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Equal(-1, RoomID);
        }

        // User Story 1 - Case 5
        [Fact]
        public void FindAvailableRoom_StartDateAfterFullyOccupiedAndEndDateAfterFullyOccupied_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(21);
            DateTime endDate = DateTime.Today.AddDays(22);
            // Act
            int RoomID = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.NotEqual(-1, RoomID);
        }

        // User Story 1 - Case 6
        [Fact]
        public void FindAvailableRoom_StartDateBeforeFullyOccupiedAndEndDateInFullyOccupied_RoomIdMinusOne()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(9);
            DateTime endDate = DateTime.Today.AddDays(11);
            // Act
            int RoomID = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Equal(-1, RoomID);
        }

        // User Story 1 - Case 7
        [Fact]
        public void FindAvailableRoom_StartDateInFullyOccupiedAndEndDateInFullyOccupied_RoomIdMinusOne()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(11);
            DateTime endDate = DateTime.Today.AddDays(19);
            // Act
            int RoomID = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Equal(-1, RoomID);
        }

        // User Story 1 - Case 8
        [Fact]
        public void FindAvailableRoom_StartDateInFullyOccupiedAndEndDateAfterFullyOccupied_RoomIdMinusOne()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(19);
            DateTime endDate = DateTime.Today.AddDays(21);
            // Act
            int RoomID = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Equal(-1, RoomID);
        }

        // User Story 2 - Case 1
        [Fact]
        public void GetFullyOccupiedDates_StartDateIsAfterEndDate_ThrowsAurgumentException()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today.AddDays(-2);
            // Act
            Action act = () => bookingManager.GetFullyOccupiedDates(startDate, endDate);
            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        // User Story 2 - Case 2 (Case 2 from User Story 1 Case 2 is not relevant to this issue, which means this  approach is like User Story 1 Case 3)
        [Fact]
        public void GetFullyOccupiedDates_StartDateBeforeFullyOccupiedAndEndDateBeforeFullyOccupied_FullyOccupiedCountIsZero()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(2);
            // Act
            List<DateTime> FullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);
            // Assert
            Assert.Empty(FullyOccupiedDates);
        }

        // User Story 2 - Case 3
        [Fact]
        public void GetFullyOccupiedDates_FullyOccupiedDatesAndNoFullyOccupiedDates_FullyOccupiedCountIsNotZero()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(9);
            DateTime endDate = DateTime.Today.AddDays(21);
            // Act
            List<DateTime> FullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);
            // Assert
            Assert.NotEmpty(FullyOccupiedDates);
        }

        // User Story 2 - Case 4
        [Fact]
        public void GetFullyOccupiedDates_StartDateAfterFullyOccupiedAndEndDateAfterFullyOccupied_FullyOccupiedCountIsZero()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(21);
            DateTime endDate = DateTime.Today.AddDays(22);
            // Act
            List<DateTime> FullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);
            // Assert
            Assert.Empty(FullyOccupiedDates);
        }

        // User Story 2 - Case 5
        [Fact]
        public void GetFullyOccupiedDates_StartDateBeforeFullyOccupiedAndEndDateInFullyOccupied_FullyOccupiedCountIsNotZero()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(9);
            DateTime endDate = DateTime.Today.AddDays(11);
            // Act
            List<DateTime> FullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);
            // Assert
            Assert.NotEmpty(FullyOccupiedDates);
        }

        // User Story 2 - Case 6
        [Fact]
        public void GetFullyOccupiedDates_StartDateInFullyOccupiedAndEndDateInFullyOccupied_FullyOccupiedCountIsNotZero()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(11);
            DateTime endDate = DateTime.Today.AddDays(19);
            // Act
            List<DateTime> FullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);
            // Assert
            Assert.NotEmpty(FullyOccupiedDates);
        }

        // User Story 2 - Case 7
        [Fact]
        public void GetFullyOccupiedDates_StartDateInFullyOccupiedAndEndDateAfterFullyOccupied_FullyOccupiedCountIsNotZero()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(19);
            DateTime endDate = DateTime.Today.AddDays(21);
            // Act
            List<DateTime> FullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);
            // Assert
            Assert.NotEmpty(FullyOccupiedDates);
        }
    }
}