using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using BandTrackerApp.Models;

namespace BandTrackerApp.Controllers
{
  public class VenuesController : Controller
  {
    [HttpGet("/venues")]
    public ActionResult VenueForm()
    {
      return View();
    }

    [HttpGet("/venues/list")]
    public ActionResult VenueList()
    {
      List<Venue> allVenues = Venue.GetAll();
      return View (allVenues);
    }

    [HttpGet("/venue/{id}/details")]
    public ActionResult VenueDetails(int id)
    {
      Venue newVenue = Venue.Find(id);
      return View(newVenue);
    }

    [HttpPost("/venues/new")]
    public ActionResult Create()
    {
      Venue newVenue = new Venue(Request.Form["venueName"],Request.Form["venueAddress"]);
      newVenue.Save();
      List<Venue> venues = Venue.GetAll();
      return View("VenueList", venues);
    }
  }
}
