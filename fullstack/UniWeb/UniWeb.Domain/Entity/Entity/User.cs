namespace UniWeb.Entities.Entity.Entity;

public class User
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public bool? Verified { get; set; }

    public string? Role { get; set; }
    
}