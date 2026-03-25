using Microsoft.AspNetCore.Mvc;

namespace erp_1.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("Test")]
    public IActionResult Test()
    {
        return View();
    }
}
