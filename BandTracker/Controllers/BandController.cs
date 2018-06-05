using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using BandTrackerApp.Models;

namespace BandTrackerApp.Controllers
{
  public class BandController : Controller
  {
    [HttpGet("/bands")]
    public ActionResult CreateForm()
    {
      List<Band> bands = Band.GetAll();
      return View(bands);
    }

    [HttpGet("/bands/list")]
    public ActionResult BandsList()
    {
      List<Band> bands = Band.GetAll();
      return View (bands);
    }

    [HttpGet("/bands/new")]
    public ActionResult BandForm()
    {
      return View();
    }

    [HttpPost("/bands/new")]
    public ActionResult Create()
    {
      Band newBand = new Band(Request.Form["bandName"],Request.Form["bandDate"]);
      newBand.Save();
      List<Band> bands = Band.GetAll();
      return View("BandList", bands);
    }

    [HttpGet("/{id}/details")]
    public ActionResult BandDetails(int id)
    {
      Band newBand = Band.Find(id);
      return View(newBand);
    }
  }
}
