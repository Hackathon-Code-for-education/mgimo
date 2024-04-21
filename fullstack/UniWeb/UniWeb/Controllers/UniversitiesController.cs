using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniWeb.Database;
using UniWeb.Entities.Entity.Entity.University;

namespace UniWeb.Controllers;

public class UniversitiesController : Controller
{
    private readonly ILogger<UniversitiesController> _logger;

    public UniversitiesController(ILogger<UniversitiesController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Details(int id)
    {
        var university = DatabaseController.GetInstance().Universities.FirstOrDefault(x => x.Id == id);
        
        return View(university);
    }
}