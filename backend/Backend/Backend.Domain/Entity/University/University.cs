using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domain.Entity;

public class University
{
    [Key]
    [ForeignKey("Applicant")]
    public int Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? Address { get; set; }
    
    public string? UniversityType { get; set; }
    
     
}