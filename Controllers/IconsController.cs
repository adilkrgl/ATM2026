using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ATM2026.Models;

namespace ATM2026.Controllers;

public class IconsController : Controller
{
  public IActionResult Tabler() => View();
  public IActionResult FontAwesome() => View();
}
