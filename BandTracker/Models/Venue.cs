using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTrackerApp.Models;
using System;

namespace BandTrackerApp.Models
{
  public class Venue
    {
      private string _name;
      private string _address;
      private int _id;

      public Venue (string name, string address, int id = 0)
      {
        _name = name;
        _address = address;
        _id = id;
      }
      public string GetName()
      {
        return _name;
      }
      public string GetAddress()
      {
        return _address;
      }
      public int GetId()
      {
        return _id;
      }
      public void SetName(string newName)
      {
        _name = newName;
      }
      public void SetAddress( string newAddress)
      {
        _address = newAddress;
      }
      public override bool Equals(System.Object otherVenue)
      {
        if (!(otherVenue is Venue))
        {
          return false;
        }
        else
        {
          Venue newVenue = (Venue) otherVenue;
          bool idEquality = this.GetId() == newVenue.GetId();
          bool nameEquality = this.GetName() == newVenue.GetName();
          bool addressEquality = this.GetAddress() == newVenue.GetAddress();
          return (idEquality && nameEquality && addressEquality);
        }
      }
      public override int GetHashCode()
      {
        return this.GetName().GetHashCode();
      }
      public static List<Venue> GetAll()
      {
        List<Venue> allVenues = new List<Venue> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM venues;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int venueId = rdr.GetInt32(0);
          string venueName = rdr.GetString(1);
          string venueAddress = rdr.GetString(2);

          Venue newVenue = new Venue(venueName, venueAddress, venueId);
          allVenues.Add(newVenue);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allVenues;
      }
      public void Save()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO venues (name, address) VALUES (@name, @address);";

        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@name";
        name.Value = this._name;
        cmd.Parameters.Add(name);

        MySqlParameter address = new MySqlParameter();
        address.ParameterName = "@address";
        address.Value = this._address;
        cmd.Parameters.Add(address);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }
      public static Venue Find(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM venues WHERE id = (@searchId);";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = id;
        cmd.Parameters.Add(searchId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int venueId = 0;
        string venueName = "";
        string venueAddress = "";

        while(rdr.Read())
        {
          venueId = rdr.GetInt32(0);
          venueName = rdr.GetString(1);
          venueAddress = rdr.GetString(2);
        }

        Venue newVenue= new Venue(venueName, venueAddress, venueId);
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return newVenue;
      }
      public void UpdateName(string newName)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE venues SET name = @newName WHERE id = @searchId;";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = _id;
        cmd.Parameters.Add(searchId);

        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@newName";
        name.Value = newName;
        cmd.Parameters.Add(name);

        cmd.ExecuteNonQuery();
        _name = newName;
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }
      public void Delete()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM bands WHERE id = @BandId; DELETE FROM vanues_bands WHERE band_id = @BandId;";

        MySqlParameter bandIdParameter = new MySqlParameter();
        bandIdParameter.ParameterName = "@BandId";
        bandIdParameter.Value = this.GetId();
        cmd.Parameters.Add(bandIdParameter);

        cmd.ExecuteNonQuery();
        if (conn != null)
        {
            conn.Close();
        }
      }
      public void AddBand(Band newBand)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO venues_bands (band_id, venue_id) VALUES (@BandId, @VenueId);";

        MySqlParameter band_id = new MySqlParameter();
        band_id.ParameterName = "@BandId";
        band_id.Value = newBand.GetId();
        cmd.Parameters.Add(band_id);

        MySqlParameter venue_id = new MySqlParameter();
        venue_id.ParameterName = "@VenueId";
        venue_id.Value = _id;
        cmd.Parameters.Add(venue_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }
      public List<Band> GetBands()
      {
         MySqlConnection conn = DB.Connection();
         conn.Open();
         MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
         cmd.CommandText = @"SELECT band_id.* FROM venues
             JOIN venues_bands ON (venues.id = venues_bands.venue_id)
             JOIN bands ON (venues_bands.band_id = bands.id)
             WHERE venues.id = @VenueId;";

         MySqlParameter venueIdParameter = new MySqlParameter();
         venueIdParameter.ParameterName = "@VenueId";
         venueIdParameter.Value = _id;
         cmd.Parameters.Add(venueIdParameter);

         MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

         List<Band> bands = new List<Band>{};
         while(rdr.Read())
         {
           int bandId = rdr.GetInt32(0);
           string bandName = rdr.GetString(1);
           string bandShowDate = rdr.GetString(2);
           Band newBand = new Band(bandName, bandShowDate, bandId);
           bands.Add(newBand);
         }
         conn.Close();
         if (conn != null)
         {
             conn.Dispose();
         }
         return bands;
     }
      public static void DeleteAll()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM venues;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }
    }
}
