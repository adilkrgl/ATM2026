using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Artiligence.InvoiceSystem.Web.Models;

namespace Artiligence.InvoiceSystem.Web.Controllers;

public class FormLayoutsController : Controller
{
public IActionResult Horizontal() => View();
public IActionResult Vertical() => View();
public IActionResult Sticky() => View();
}
