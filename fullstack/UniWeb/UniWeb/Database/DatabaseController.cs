using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UniWeb.Entities.Entity.Entity;
using UniWeb.Entities.Entity.Entity.University;

namespace UniWeb.Database;

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
    
    /// <summary>
    /// Все пользователи системы, которые имеют право что-либо делать
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Все зарегистрированные студентый системы
    /// </summary>
    public DbSet<Student> Students { get; set; }

    /// <summary>
    /// Все отзывы об университетах
    /// </summary>
    public DbSet<UniversityReview> UniversityReviews { get; set; }
    
    #endregion
    
    #region Constructor
    
    private DatabaseController()
    {
#if DEBUG
        // Database.EnsureDeleted();
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

    /// <summary>
    /// Добавление настроек модуля
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Applicant>(x =>
        {
            x.HasKey(y => y.Id);
            x.Property(y => y.FavoriteUniversities).HasConversion(x => JsonConvert.SerializeObject(x), // to converter
                x => JsonConvert.DeserializeObject<List<int>>(x));
        });
        
    }
    
    #endregion
}