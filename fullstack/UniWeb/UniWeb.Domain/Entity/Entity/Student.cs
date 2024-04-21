namespace UniWeb.Entities.Entity.Entity;

public class Student
{
    public int Id { get; set; }
    
    public string? Login { get; set; }
    
    public string? Password { get; set; }
    
    public string? Name { get; set; }
    
    public string? Surname { get; set; }
    
    public int Course { get; set; }
    
    public string? Faculty { get; set; }
    
    public string? TelegramUserName { get; set; }
    
    public int? UniversityId { get; set; }
    
    public string? StudentCardId { get; set; }
    
    public bool IsVerified { get; set; }
    
    public string? VerificationCode { get; set; }
}