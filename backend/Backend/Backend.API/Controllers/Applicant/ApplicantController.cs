using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend.API.Audentification;
using Backend.API.Database;
using Backend.Core.Audentification;
using Backend.Core.Helpers;
using Backend.Domain.WebModels;
using Backend.Domain.WebModels.Applicant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

    /// <summary>
    /// Позволяет получить данные для аудентификации пользователя в системе
    /// </summary>
    private readonly ApplicantAudentificationService _authService;
    
    #endregion

    #region Basic Constructor

    /// <summary>
    /// Контроллер идентификации пользователя в системе абитуриента
    /// </summary>
    /// <param name="logger"></param>
    public ApplicantController(ILogger<ApplicantController> logger)
    {
        _logger = logger;
        _authService = new ApplicantAudentificationService();
    }

    #endregion

    #region HTTP Methods

    [HttpPost]
    [ActionName("Register")]
    public async Task<IActionResult> Register([FromForm]ApplicantRegister registerModel)
    {
        if (string.IsNullOrWhiteSpace(registerModel.Email) || string.IsNullOrWhiteSpace(registerModel.Password))
        {
            return BadRequest("No content in model");
        }
        var user = DatabaseController.GetInstance().Applicants.FirstOrDefault(x =>
            x.Login == registerModel.Email);

        // Проверка пользователя на существование в системе
        if (user != null) return BadRequest("Пользователь уже существует");

        var newUser = new Domain.Entity.Applicant()
        {
            Email = registerModel.Email,
            Login = registerModel.Email,
            Password = Sha256Helper.ToHash(registerModel.Password),
            Phone = registerModel.Phone,
            Role = "Applicant"
        };
        
        // Регистрируем пользователя в системе
        DatabaseController.GetInstance().Applicants.Add(newUser);
        await DatabaseController.GetInstance().SaveChangesAsync();

        var identity = _authService.GetIdentity(newUser.Email, newUser.Role, newUser.Id);
        
        var now = DateTime.UtcNow;
        
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        
        var response = new
        {
            ServerTime = now,
            Login = newUser.Login,
            Role = "Applicant",
            Token = new JwtSecurityTokenHandler().WriteToken(jwt)
        };

        return Ok(response);
    }

    /// <summary>
    /// Авторизация пользователя в системе
    /// </summary>
    /// <param name="loginModel">Модель авторизационных данных пользователя</param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Login")]
    public async Task<IActionResult> Login([FromForm]ApplicantLogin loginModel)
    {
        if (string.IsNullOrWhiteSpace(loginModel.Login) || string.IsNullOrWhiteSpace(loginModel.Password))
        {
            return BadRequest("No content in model");
        }

        var user = await DatabaseController.GetInstance().Applicants.FirstOrDefaultAsync(x =>
            x.Login == loginModel.Login &&
            x.Password == Sha256Helper.ToHash(loginModel.Password));

        // Пользователь не найден в системе
        if (user == null)
        {
            return BadRequest("User is not found");
        }
        
        var identity = _authService.GetIdentity(user.Login, user.Role, user.Id);
        var now = DateTime.UtcNow;
        
        // Создаем JWT-токен для авторизации пользователя в системе
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        // Возвращаем успешный ответ авторизации пользователя
        return Ok(new
        {
            ServerTime = now,
            Login = user.Login,
            Role = "Applicant",
            Token = new JwtSecurityTokenHandler().WriteToken(jwt)
        });
    } 
    
    [HttpGet]
    [Authorize]
    [ActionName("Me")]
    public async Task<IActionResult> GetMe()
    {
        if (!User.Identity.IsAuthenticated) return BadRequest("User is not authenticated");

        var user = await DatabaseController.GetInstance().Applicants
            .FirstOrDefaultAsync(x => x.Login == User.Identity.Name);

        return Ok(
            new
            {
                Login = user.Login,
                Role = user.Role,
                Phone = user.Phone,
                Email = user.Email
            }
            );

    }

    #endregion

    
}