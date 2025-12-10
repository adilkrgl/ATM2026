using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Artiligence.InvoiceSystem.Web.Models;

namespace Artiligence.InvoiceSystem.Web.Controllers;

public class LogisticsController : Controller
{
  public IActionResult Dashboard() => View();
  public IActionResult Fleet() => View();
}
