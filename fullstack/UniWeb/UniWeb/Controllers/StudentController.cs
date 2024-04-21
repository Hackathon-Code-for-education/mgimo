using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniWeb.Core.Helpers;
using UniWeb.Core.Services.Email;
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

    #region Login

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginStudentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = DatabaseController.GetInstance().Students
                .FirstOrDefault(x => x.Login == model.Email);
            
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                return View(model);
            }
            
            if (user.Password != Sha256Helper.ToHash(model.Password))
            {
                ModelState.AddModelError(string.Empty, "Неверный пароль");
                return View(model);
            }
            
            if (!user.IsVerified)
            {
                ModelState.AddModelError(string.Empty, "Ваш аккаунт находится на верификации, ожидайте!");
                return View(model);
            }
            
            // Добавляем почту, индентификатор пользователя в системе и его роль 
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Login),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "Student")
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
    
    #region Register

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

            // Проверяем почту на наличие ее в базе данных
            var mailDomain = model.Email.Split("@");
            if (!DatabaseController.GetInstance().Universities.Any(x => x.EmailDomain == mailDomain[1]))
            {
                ModelState.AddModelError(string.Empty, "Данная почта не является студенческой и не относится ни к одному университету");
                return View(model);
            }

            // TODO Добавить поля имя и фамилия в таблице для обширной инфоримации о пользователе
            var verificationToken = Guid.NewGuid().ToString();
            var student = new Student()
            {
                Login = model.Email,
                Password = Sha256Helper.ToHash(model.Password),
                Name = model.Name,
                Surname = model.Surname,
                Course = model.Course,
                Faculty = model.Faculty,
                IsVerified = false,
                VerificationCode = Sha256Helper.ToHash(verificationToken),
                UniversityId = DatabaseController.GetInstance()
                    .Universities
                    .FirstOrDefault(x=> x.EmailDomain == mailDomain[1])?.Id, 
            };
            
            DatabaseController.GetInstance().Students.Add(student);
            await DatabaseController.GetInstance().SaveChangesAsync();
            
            //// Добавляем почту, индентификатор пользователя в системе и его роль 
            //var claims = new List<Claim>()
            //{
            //    new Claim(ClaimTypes.Email, student.Login),
            //    new Claim(ClaimTypes.Sid, student.Id.ToString()),
            //    new Claim(ClaimTypes.Role, "Student")
//
            //};
            
            // // Добавление идентификаторов пользователя в системе
            // var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
// 
            // // Срок действия подключения - 7 дней
            // var authProperties = new AuthenticationProperties()
            // {
            //     IsPersistent = true,
            //     ExpiresUtc = DateTime.UtcNow.AddDays(7)
            // };
// 
            // // Основной метод регистрации пользователя в системе
            // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            //     new ClaimsPrincipal(claimIdentity), authProperties);
// 
            // Отправялем пользователя на главную страницу

            var emailService = new EmailServiceVerification();
#if DEBUG
            emailService.SendVerificationEmail(model.Email,
                $"http://localhost:5051/student/verify?email={model.Email}&token={verificationToken}");
#endif
#if !DEBUG
            emailService.SendVerificationEmail(model.Email,
                $"https://api.mgimoapp.ru/student/verify?email={model.Email}&token={verificationToken}");
#endif
            
            return RedirectToAction("Verify", "Student", new {email = model.Email});
        }
        
        ModelState.AddModelError(string.Empty, "Проверьте правильность ввода данных");

        return View(model);
    }

    #endregion

    #region Verification

    public IActionResult Verify(string? email, string? token)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                var mailDomain = email.Split("@"); 
                return View(model:new String($"На почту {email} был отправлен ссылка для подтверждения! \n\n Вам повезло, у нас есть ваш университет: {DatabaseController.GetInstance().Universities.FirstOrDefault(x=> x.EmailDomain == mailDomain[1])?.Name} \ud83e\udd29"));
            }
            
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(token))
            {
                

                return RedirectToAction("Index", "Home");
            }
            
            var student = DatabaseController.GetInstance().Students.FirstOrDefault(x => x.Login == email);

            
            if (student == null)
                return RedirectToAction("Index", "Home");

            if (student.IsVerified)
                return View(model:new String("Ваш аккаунт уже авторизирован! Вы можете пользоваться всеми услугами \ud83e\udd29"));
            
            
            if (student.VerificationCode != Sha256Helper.ToHash(token))
                return View(model:new String("Код верификации неверен! \ud83d\udeab"));

            student.IsVerified = true;
            DatabaseController.GetInstance().SaveChanges();
            
            return View(model:new String("Вы успешно верифицировали аккаунт! \ud83e\udd29"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Index", "Home");
        }
    }

    #endregion
    
}