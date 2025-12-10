using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Artiligence.InvoiceSystem.Web.Models;

namespace Artiligence.InvoiceSystem.Web.Controllers;

public class IconsController : Controller
{
  public IActionResult Tabler() => View();
  public IActionResult FontAwesome() => View();
}
