using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using UniWeb.Core.Helpers;
using UniWeb.Database;
using UniWeb.Entities.Entity.Entity;
using UniWeb.Entities.WebModels.Student;

namespace UniWeb.Controllers;

public class StudentController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Регистрация студента в системе
    /// </summary>
    /// <returns></returns>
    ///
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegistrationStudentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = DatabaseController.GetInstance().Users
                .FirstOrDefault(x => x.Login == model.Email);
            
            if (user != null)
            {
                ModelState.AddModelError(string.Empty, "Данный пользователь уже существует");
                return View(model);
            }

            // TODO Добавить поля имя и фамилия в таблице для обширной инфоримации о пользователе
            var student = new Student()
            {
                Login = model.Email,
                Password = Sha256Helper.ToHash(model.Password),
                Name = model.Name,
                Surname = model.Surname,
                Course = model.Course,
                Faculty = model.Faculty,
                IsVerified = false,
                VerificationCode = Sha256Helper.ToHash(Guid.NewGuid().ToString()),
                UniversityId = int.Parse(model.UniversityId), 
            };
            
            DatabaseController.GetInstance().Students.Add(student);
            await DatabaseController.GetInstance().SaveChangesAsync();
            
            // Добавляем почту, индентификатор пользователя в системе и его роль 
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, student.Login),
                new Claim(ClaimTypes.Sid, student.Id.ToString()),
                new Claim(ClaimTypes.Role, "Student")

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
}