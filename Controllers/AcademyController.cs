using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Artiligence.InvoiceSystem.Web.Models;

namespace Artiligence.InvoiceSystem.Web.Controllers;

public class AcademyController : Controller
{
  public IActionResult Dashboard() => View();
  public IActionResult MyCourse() => View();
  public IActionResult CourseDetails() => View();
}
