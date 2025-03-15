using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;

public class StudentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
