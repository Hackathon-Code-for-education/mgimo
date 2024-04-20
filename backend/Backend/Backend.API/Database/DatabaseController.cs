using Backend.Domain.Entity;
using Microsoft.EntityFrameworkCore;


namespace Backend.API.Database;

public class DatabaseController : DbContext
{
    #region Singleton

    private static DatabaseController _instance;
    public static DatabaseController GetInstance()
    {
        return _instance ?? (_instance = new DatabaseController());
    }

    #endregion

    #region Public Tables

    /// <summary>
    /// Все абитуриенты системы
    /// </summary>
    public DbSet<Applicant> Applicants { get; set; }
    
    /// <summary>
    /// Все университеты системы
    /// </summary>
    public DbSet<University> Universities { get; set; }

    #endregion
    
    #region Constructor
    
    private DatabaseController()
    {
#if DEBUG
        Database.EnsureDeleted();
        Database.EnsureCreated();
#endif
#if !DEBUG
        Database.EnsureCreated();
#endif
    }
    
    #endregion

    #region Protected Methods

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(
            $"server=158.160.45.67;" +
            $"user=api;" +
            $"password=JLgHe5Vs;" +
            $"database=uni;",
            new MySqlServerVersion(new Version(8, 0, 33)));

        // optionsBuilder.UseMySql(
        //     $"server={ConfigurationManager.AppSettings["Database:Server"]};" +
        //     $"user={ConfigurationManager.AppSettings["Database:Username"]};" +
        //     $"password={ConfigurationManager.AppSettings["Database:Password"]};" +
        //     $"database={ConfigurationManager.AppSettings["Database:DatabaseName"]};", 
        //     new MySqlServerVersion(new Version(8, 0, 33))
        // );
    }

    #endregion
}