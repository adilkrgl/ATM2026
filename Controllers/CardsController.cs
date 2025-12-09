using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ATM2026.Models;

namespace ATM2026.Controllers;

public class CardsController : Controller
{
  public IActionResult Basic() => View();
  public IActionResult Advance() => View();
  public IActionResult Statistics() => View();
  public IActionResult Analytics() => View();
  public IActionResult Actions() => View();
  public IActionResult Gamifications() => View();
}
