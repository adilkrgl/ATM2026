using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Artiligence.InvoiceSystem.Web.Models;

namespace Artiligence.InvoiceSystem.Web.Controllers;

public class AccessController : Controller
{
public IActionResult Permission() => View();
public IActionResult Roles() => View();
}
