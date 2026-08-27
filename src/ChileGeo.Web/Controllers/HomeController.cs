using Microsoft.AspNetCore.Mvc;

namespace ChileGeo.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Regiones");
}
