using System.ComponentModel.DataAnnotations;

namespace UniWeb.Entities.WebModels.Applicant;

public class ApplicantRegisterViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
 
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string? PasswordConfirm { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    public string? Surname { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    public string? Name { get; set; }
    
    
    [Required]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }
    
}