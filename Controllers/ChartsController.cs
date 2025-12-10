using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Artiligence.InvoiceSystem.Web.Models;

namespace Artiligence.InvoiceSystem.Web.Controllers;

public class ChartsController : Controller
{
  public IActionResult Apex() => View();
  public IActionResult Chartjs() => View();
}
