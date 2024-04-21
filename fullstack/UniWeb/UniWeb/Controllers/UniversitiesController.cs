using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniWeb.Database;
using UniWeb.Entities.Entity.Entity.University;
using UniWeb.Entities.WebModels.University;

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

    [HttpPost]
    [Authorize(Roles = "Student")]
    public IActionResult AddReview([FromForm]AddReviewViewModel? model)
    {
        if (model != null)
        {
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;

            var user = DatabaseController.GetInstance().Students.FirstOrDefault(x => x.Id == int.Parse(userId));

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            DatabaseController.GetInstance().UniversityReviews.Add(
                new UniversityReview()
                {
                    Date = DateTime.Now.Date.ToString(),
                    Text = model.Text,
                    Name = user.Name,
                    Surname = user.Surname,
                    FirstLetters = user.Name.ToCharArray()[0].ToString() + user.Surname.ToCharArray()[0],
                    Stars = model.Stars,
                    UniversityId = user.UniversityId
                });
            DatabaseController.GetInstance().SaveChanges();

            return Ok();
        }
        
        return BadRequest("Error");
                
    }
    
    
    
}