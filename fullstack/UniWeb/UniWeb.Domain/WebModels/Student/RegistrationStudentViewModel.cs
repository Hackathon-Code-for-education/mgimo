using System.ComponentModel.DataAnnotations;

namespace UniWeb.Entities.WebModels.Student;

public class RegistrationStudentViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
 
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [Required]
    public string? Name { get; set; }
    
    [Required]
    public string? Surname { get; set; }
    
    
    public int Course { get; set; }
    
    
    public string? Faculty { get; set; }
    
    
    public string? TelegramUserName { get; set; }
    
    
    public string? UniversityId { get; set; }
    
    
    public string? StudentCardId { get; set; }
    
}