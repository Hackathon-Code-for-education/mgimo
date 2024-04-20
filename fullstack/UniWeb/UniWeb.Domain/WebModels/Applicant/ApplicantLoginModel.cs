using System.ComponentModel.DataAnnotations;

namespace UniWeb.Entities.WebModels.Applicant;

public class ApplicantLoginModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
 
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}