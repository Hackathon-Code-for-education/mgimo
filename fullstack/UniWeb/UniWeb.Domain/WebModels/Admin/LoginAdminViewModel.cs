using System.ComponentModel.DataAnnotations;

namespace UniWeb.Entities.WebModels.Admin;

public class LoginAdminViewModel
{
    [Required]
    [DataType(DataType.Text)]
    public string? Login { get; set; }
 
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}