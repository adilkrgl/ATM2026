using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Artiligence.InvoiceSystem.Web.Models;

namespace Artiligence.InvoiceSystem.Web.Controllers;

public class FormWizardController : Controller
{
public IActionResult Icons() => View();
public IActionResult Numbered() => View();
}
