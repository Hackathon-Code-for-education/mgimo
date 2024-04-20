namespace UniWeb.Entities.Entity.Entity;

public class Applicant
{
    /// <summary>
    /// Уникальный идентификатор пользователя в системе
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string? Login { get; set; }
    
    /// <summary>
    /// Пароль пользователя хранится в базе в хешированном варианте SHA256
    /// </summary>
    public string? Password { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Role { get; set; }
}