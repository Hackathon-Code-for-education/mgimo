using System.ComponentModel.DataAnnotations;

namespace UniWeb.Entities.WebModels.University;

public class AddReviewViewModel
{

    public int Stars { get; set; }
    
    public string? Text { get; set; }
}