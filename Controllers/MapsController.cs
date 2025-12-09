using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ATM2026.Models;

namespace ATM2026.Controllers;

public class MapsController : Controller
{
  public IActionResult Leaflet() => View();
}
