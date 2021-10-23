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
        public void GetSpaces()
        {
            // Arrange
            SpaceDAO dao = new SpaceDAO(ConnectionString);
            VenueDAO venueDao = new VenueDAO(ConnectionString);
            Venue venue = venueDao.SelectVenue(1);

            // Act
            ICollection<Space> result = dao.GetSpaces(venue);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }
    }
}
