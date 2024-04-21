using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniWeb.Core.Helpers;
using UniWeb.Database;
using UniWeb.Entities.WebModels.Admin;

namespace UniWeb.Controllers.Admin;

public class AdminController : Controller
{
    // GET
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Users()
    {
        return View();
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult Verification()
    {
        return View();
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult Universities()
    {
        return View();
    }

    #region Login

    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginAdminViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = DatabaseController.GetInstance().Users
                .FirstOrDefault(x => x.Login == model.Login && x.Password == Sha256Helper.ToHash(model.Password));

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
                new Claim(ClaimTypes.Role, "Admin")
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
            return RedirectToAction("Index", "Admin");
        }
        
        ModelState.AddModelError(string.Empty, "Проверьте правильность ввода логина и пароля");
        
        return View(model);
    }

    #endregion

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Logout()
    {
        // Удаление кукисов
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
 
        // Redirect to the login page or home page
        return RedirectToAction("Index", "Home");
    }
}