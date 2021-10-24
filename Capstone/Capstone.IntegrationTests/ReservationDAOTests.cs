using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class ReservationDAOTests : IntegrationTestBase
    {
        [TestMethod]
        [DataRow("01/01/2021", 2, 2, 1, 1)]
        [DataRow("07/06/2021", 2, 2, 1, 1)]
        [DataRow("07/06/2021", 2, 2, 2, 2)]
        [DataRow("01/01/2021", 2, 2, 2, 1)]
        public void ReserveASpace_ReturnsCorrectAmountOfOpeningSpaces(string date, int days, int attendees, int selectedVenue, int expected)
        {
            // Arrange
            ReservationDAO dao = new ReservationDAO(ConnectionString);
            VenueDAO venueDao = new VenueDAO(ConnectionString);
            Venue venue = venueDao.SelectVenue(selectedVenue, venueDao.GetVenue());

            // Act
            ICollection<Reservation> result = dao.ReserveASpace(Convert.ToDateTime(date), days, attendees, venue);

            // Assert
            Assert.AreEqual(expected, result.Count);
        }

        [TestMethod]
        public void MakeReservation_Tests()
        {
            // Arrange
            ReservationDAO dao = new ReservationDAO(ConnectionString);
            Reservation reservation = new Reservation
            {                
                SpaceId = 1,
                NumberOfAttendees = 2,
                StartDate = Convert.ToDateTime("01/22/2021"),
                EndDate = Convert.ToDateTime("01/25/2021"),               
            };
            // Act
            int id = dao.MakeReservation(reservation, "John");
            // Assert
            Assert.IsTrue(id > 1);
           
        }
    }
}
