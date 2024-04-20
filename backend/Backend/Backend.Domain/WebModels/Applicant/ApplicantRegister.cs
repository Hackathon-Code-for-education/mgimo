namespace Backend.Domain.WebModels.Applicant;

public class ApplicantRegister
{
    public string? Email { get; set; }
    
    public string? Name { get; set; }
    
    public string? Surname { get; set; }
    
    public string? Password { get; set; }
    
    public string? PasswordConfirm { get; set; } 
    
    public string? Phone { get; set; }
}