using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using BandTrackerApp.Models;
using System;

namespace BandTrackerApp.Tests
{
  [TestClass]
  public class VenueTest : IDisposable
  {
    public VenueTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker_test;";
    }
    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }
    [TestMethod]
    public void Equals_TrueForSameProperties_Band()
     {
       //Arrange, Act
       Venue firstVenue  = new Venue ("Global", "310 S. 1st");
       Venue secondVenue  = new Venue ("Global", "310 S. 1st");

       //Assert
       Assert.AreEqual(firstVenue, secondVenue);
     }
  }
}
