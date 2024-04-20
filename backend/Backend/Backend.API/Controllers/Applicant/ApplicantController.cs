using Backend.API.Database;
using Backend.Domain.WebModels;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers.Applicant;

[ApiController]
[Route("[controller]/[action]")]
public class ApplicantController : ControllerBase
{
    #region Private Logger

    /// <summary>
    /// Логгер для логирования операций системы
    /// </summary>
    private readonly ILogger<ApplicantController> _logger;

    #endregion

    #region Basic Constructor

    /// <summary>
    /// Контроллер идентификации пользователя в системе абитуриента
    /// </summary>
    /// <param name="logger"></param>
    public ApplicantController(ILogger<ApplicantController> logger)
    {
        _logger = logger;
    }

    #endregion
    

    [HttpPost]
    [ActionName("Login")]
    public IActionResult Login(ApplicantLogin? loginData)
    {
        if (loginData == null)
        {
            return NoContent();
        }

        return Ok();
    }

    [HttpGet]
    public IActionResult Get()
    {
        var users = DatabaseController.GetInstance().Applicants.ToList();

        return Ok(users);
    }
}