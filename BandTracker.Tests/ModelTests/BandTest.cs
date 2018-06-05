using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using BandTrackerApp.Models;
using System;

namespace BandTrackerApp.Tests
{
  [TestClass]
  public class BandTest : IDisposable
  {
    public BandTest()
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
      Band firstBand  = new Band ("Fleet Foxes", "03/14/2016");
      Band secondBand  = new Band ("Fleet Foxes", "03/14/2016");

      //Assert
      Assert.AreEqual(firstBand, secondBand );
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Band.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      Band testBand  = new Band ("Fleet Foxes", "03/14/2016");
      testBand.Save();

      //Act
      Band savedBand = Band.GetAll()[0];

      int result = savedBand.GetId();
      int testId = testBand.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    // [TestMethod]
    public void Find_FindsBandInDatabase_Item()
    {
      //Arrange
      Band testBand = new Band("Fleet Foxes", "03/14/2016");
      testBand.Save();

      //Act
      Band result = Band.Find(testBand.GetId());

      //Assert
      Assert.AreEqual(testBand, result);
    }
  }
}
