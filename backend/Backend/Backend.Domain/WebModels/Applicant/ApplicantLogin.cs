namespace Backend.Domain.WebModels.Applicant;

public class ApplicantLogin
{
    /// <summary>
    /// Логин пользователя в системе
    ///
    /// Может быть номером телефона или почтой
    /// </summary>
    public string? Login { get; set; }
    
    /// <summary>
    /// "Чистый пароль" пользователя в системе
    /// </summary>
    public string? Password { get; set; }
}