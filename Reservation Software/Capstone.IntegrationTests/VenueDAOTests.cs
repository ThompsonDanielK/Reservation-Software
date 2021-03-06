using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class VenueDAOTests : IntegrationTestBase
    {
        [TestMethod]
        public void GetVenue_ReturnsCorrectAmountOfVenues()
        {
            // Arrange
            VenueDAO dao = new VenueDAO(ConnectionString);

            // Act
            ICollection<Venue> result = dao.GetVenue();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

        }

        [TestMethod]
        [DataRow(1, 2)]
        [DataRow(2, 1)]
        public void SelectVenue_ReturnsCorrectVenue(int selectedVenue, int expected)
        {
            // Arrange
            VenueDAO dao = new VenueDAO(ConnectionString);

            // Act
            Venue result = dao.SelectVenue(selectedVenue, dao.GetVenue());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.Id);
        }
    }
}
