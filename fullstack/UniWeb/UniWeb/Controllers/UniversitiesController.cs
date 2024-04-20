using Microsoft.AspNetCore.Mvc;

namespace UniWeb.Controllers;

public class UniversitiesController : Controller
{
    private readonly ILogger<UniversitiesController> _logger;

    public UniversitiesController(ILogger<UniversitiesController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    
}