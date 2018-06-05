using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTrackerApp.Models;
using System;

namespace BandTrackerApp.Models
{
  public class Band
    {
      private string _name;
      private string _showDate;
      private int _id;

      public Band(string name, string showDate, int id = 0)
      {
          _name = name;
          _showDate = showDate;
          _id = id;
      }
      public string GetName()
      {
        return _name;
      }
      public string GetShowDate()
      {
        return _showDate;
      }
      public int GetId()
      {
        return _id;
      }
      public void SetName(string newName)
      {
        _name = newName;
      }
      public void SetShowDate(string newShowDate)
      {
        _showDate = newShowDate;
      }
      public override bool Equals(System.Object otherBand)
      {
        if (!(otherBand is Band))
        {
          return false;
        }
        else
        {
          Band newBand = (Band) otherBand;
          bool idEquality = this.GetId() == newBand.GetId();
          bool nameEquality = this.GetName() == newBand.GetName();
          bool showDateEquality = this.GetShowDate() == newBand.GetShowDate();
          return (idEquality && nameEquality && showDateEquality);
        }
      }
      public override int GetHashCode()
      {
        return this.GetName().GetHashCode();
      }
      public static List<Band> GetAll()
      {
        List<Band> allBands = new List<Band> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM bands;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int bandId = rdr.GetInt32(0);
          string bandName = rdr.GetString(1);
          string bandShowDate = rdr.GetString(2);

          Band newBand = new Band(bandName, bandShowDate, bandId);
          allBands.Add(newBand);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allBands;
      }
      public void Save()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO bands (name, showDate) VALUES (@name, @showDate);";

        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@name";
        name.Value = this._name;
        cmd.Parameters.Add(name);

        MySqlParameter showDate = new MySqlParameter();
        showDate.ParameterName = "@showDate";
        showDate.Value = this._showDate;
        cmd.Parameters.Add(showDate);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }
      public static Band Find(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM bands WHERE id = (@searchId);";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = id;
        cmd.Parameters.Add(searchId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int bandId = 0;
        string bandName = "";
        string bandShowDate = "";

        while(rdr.Read())
        {
          bandId = rdr.GetInt32(0);
          bandName = rdr.GetString(1);
          bandShowDate = rdr.GetString(2);
        }

        Band newBand = new Band(bandName, bandShowDate, bandId);
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return newBand;
      }
      public void UpdateName(string newName)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE bands SET name = @newName WHERE id = @searchId;";

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
      public void AddVenue(Venue newVenue)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO venues_bands (venue_id, band_id) VALUES (@VenueId, @BandId);";

        MySqlParameter venue_id = new MySqlParameter();
        venue_id.ParameterName = "@VanueId";
        venue_id.Value = newVenue.GetId();
        cmd.Parameters.Add(venue_id);

        MySqlParameter band_id = new MySqlParameter();
        band_id.ParameterName = "@BandId";
        band_id.Value = _id;
        cmd.Parameters.Add(band_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }
      public List<Venue> GetVenues()
      {
         MySqlConnection conn = DB.Connection();
         conn.Open();
         MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
         cmd.CommandText = @"SELECT venue_id.* FROM bands
             JOIN venues_bands ON (bands.id = venues_bands.band_id)
             JOIN venues ON (venues_bands.venue_id = venues.id)
             WHERE bands.id = @BandId;";

         MySqlParameter bandIdParameter = new MySqlParameter();
         bandIdParameter.ParameterName = "@BandId";
         bandIdParameter.Value = _id;
         cmd.Parameters.Add(bandIdParameter);

         MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

         List<Venue> venues = new List<Venue>{};
         while(rdr.Read())
         {
           int venueId = rdr.GetInt32(0);
           string venueName = rdr.GetString(1);
           string venueAddress = rdr.GetString(2);
           Venue newVenue = new Venue(venueName, venueAddress, venueId);
           venues.Add(newVenue);
         }
         conn.Close();
         if (conn != null)
         {
             conn.Dispose();
         }
         return venues;
     }
     public static void DeleteAll()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM bands;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }
    }
}
