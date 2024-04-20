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
        return Ok(new DemoModel()
        {
            Username = " he;dskald;",
            Name = "sjdklasda"
        }); 
    }

    [HttpPost]
    public IActionResult Post([FromForm]DemoModel model)
    {
        return Ok(new
        {
            UserName = "hsdkasda",
            Name = "sjdalsdjla"
        });
    }
}