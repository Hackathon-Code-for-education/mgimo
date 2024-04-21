namespace UniWeb.Entities.Entity.Entity.University;

public class UniversityReview
{
    public int Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? Surname { get; set; }
    
    public string? Text { get; set; }
    
    public string? Date { get; set; }
    
    public string? FirstLetters { get; set; }
    
    public int? Stars { get; set; }
    
    public int? UniversityId { get; set; }
}