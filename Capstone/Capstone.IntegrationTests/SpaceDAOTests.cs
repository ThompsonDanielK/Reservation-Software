using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class SpaceDAOTests : IntegrationTestBase 
    {
        [TestMethod]
        [DataRow(2, 2)]
        [DataRow(1, 1)]
        public void GetSpaces(int selectedSpace, int expected)
        {
            // Arrange
            SpaceDAO dao = new SpaceDAO(ConnectionString);
            VenueDAO venueDao = new VenueDAO(ConnectionString);
            Venue venue = venueDao.SelectVenue(selectedSpace, venueDao.GetVenue());

            // Act
            ICollection<Space> result = dao.GetSpaces(venue);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.Count);
        }
    }
}
