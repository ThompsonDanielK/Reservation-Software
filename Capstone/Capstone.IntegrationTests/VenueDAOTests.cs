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
            Assert.AreEqual(1, result.Count);

        }

        [TestMethod]
        public void SelectVenue_()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
