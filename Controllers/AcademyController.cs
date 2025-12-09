using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ATM2026.Models;

namespace ATM2026.Controllers;

public class AcademyController : Controller
{
  public IActionResult Dashboard() => View();
  public IActionResult MyCourse() => View();
  public IActionResult CourseDetails() => View();
}
