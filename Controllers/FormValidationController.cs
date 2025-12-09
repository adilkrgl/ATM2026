using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ATM2026.Models;

namespace ATM2026.Controllers;

public class FormValidationController : Controller
{
public IActionResult Validation() => View();
}
