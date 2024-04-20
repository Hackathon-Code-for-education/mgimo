using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniWeb.Core.Helpers;
using UniWeb.Database;
using UniWeb.Entities.Entity.Entity;
using UniWeb.Entities.WebModels.Applicant;

namespace UniWeb.Controllers;

public class ApplicantController : Controller
{

    #region Login

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(ApplicantLoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = DatabaseController.GetInstance().Applicants
                .FirstOrDefault(x => x.Login == model.Email && x.Password == Sha256Helper.ToHash(model.Password));

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                return View(model);
            }
            
            // Добавляем почту, индентификатор пользователя в системе и его роль 
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Login),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Срок действия подключения - 7 дней
            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            };

            // Основной метод регистрации пользователя в системе
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity), authProperties);

            // Отправялем пользователя на главную страницу
            return RedirectToAction("Index", "Home");
        }
        
        ModelState.AddModelError(string.Empty, "Проверьте правильность ввода логина и пароля");
        
        return View(model);
    }

    #endregion
    
    #region LogOut

    /// <summary>
    /// Выход пользователя из системы
    /// </summary>
    /// <returns>Итоговый результат пользовательсокго выхода</returns>
    public async Task<IActionResult> Logout()
    {
        // Удаление кукисов
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
 
        // Redirect to the login page or home page
        return RedirectToAction("Index", "Home");
    }

    #endregion

    #region Register

    public IActionResult Register()
    {
        return View();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Register(ApplicantRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = DatabaseController.GetInstance().Applicants
                .FirstOrDefault(x => x.Login == model.Email);
            if (user != null)
            {
                ModelState.AddModelError(string.Empty, "Данный пользователь уже существует");
                return View(model);
            }

            // TODO Добавить поля имя и фамилия в таблице для обширной инфоримации о пользователе
            var applicant = new Applicant()
            {
                Login = model.Email,
                Password = Sha256Helper.ToHash(model.Password),
                Phone = model.Phone,
                Email = model.Email,
                Role = "Applicant"
            };
            
            DatabaseController.GetInstance().Applicants.Add(applicant);
            await DatabaseController.GetInstance().SaveChangesAsync();
            
            // Добавляем почту, индентификатор пользователя в системе и его роль 
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, applicant.Login),
                new Claim(ClaimTypes.Sid, applicant.Id.ToString()),
                new Claim(ClaimTypes.Role, applicant.Role)
            };
            
            // Добавление идентификаторов пользователя в системе
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Срок действия подключения - 7 дней
            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            };

            // Основной метод регистрации пользователя в системе
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity), authProperties);

            // Отправялем пользователя на главную страницу
            return RedirectToAction("Index", "Home");

        }
        
        ModelState.AddModelError(string.Empty, "Проверьте правильность ввода данных");

        return View(model);
    }

    #endregion

    #region Forgot Password

    public IActionResult ForgotPassword()
    {
        return View();
    }    

    #endregion

    #region About User

    public IActionResult About()
    {
        if (User.Identity.IsAuthenticated)
        {
            
        }
        return View();
    }

    #endregion
    
}