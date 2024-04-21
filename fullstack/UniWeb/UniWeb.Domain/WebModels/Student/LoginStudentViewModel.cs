using System.ComponentModel.DataAnnotations;

namespace UniWeb.Entities.WebModels.Student;

public class LoginStudentViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
 
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}