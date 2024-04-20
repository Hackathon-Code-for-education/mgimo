using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        
        
    }

    [HttpGet]
    public IActionResult Get(string? data)
    {
        return Ok(new 
        {
            Username = " he;dskald;",
            Name = "sjdklasda"
        }); 
    }
    
}