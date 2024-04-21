
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniWeb.Database;
using UniWeb.Entities.Entity.Entity;

namespace UniWeb.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        // DatabaseController.GetInstance().Applicants.Add(new Applicant()
        // {
        //     Login = "admin@yandex.ru",
        //     Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
        //     Email = "admin@yandex.ru",
        //     Role = "Admin",
        //     FavoriteUniversities = new List<int>()
        //     {
        //         1,2
        //     }
        // });
        // DatabaseController.GetInstance().SaveChanges();
        
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    
}