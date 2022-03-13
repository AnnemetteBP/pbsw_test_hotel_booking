using System;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;

        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

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
        public void FindAvailableRoom_StartDateNotInTheFutureAndEndDateInTheFuture_ArgumentExeption()
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
        public void FindAvailableRoom_StartDateInFullyOccupiedAndEndDateBeforeFullyOccupied_RoomIdMinusOne()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(19);
            DateTime endDate = DateTime.Today.AddDays(21);
            // Act
            int RoomID = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Equal(-1, RoomID);
        }
    }
}
