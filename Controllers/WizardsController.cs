using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ATM2026.Models;

namespace ATM2026.Controllers;

public class WizardsController : Controller
{
  public IActionResult Checkout() => View();
  public IActionResult CreateDeal() => View();
  public IActionResult PropertyListing() => View();
}
