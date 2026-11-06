using Microsoft.AspNetCore.Mvc;

namespace KodaStore.Api.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    [Route("/")]
    public IActionResult Index()
    {
        return View("~/Views/Home/Index.cshtml");
    }
}