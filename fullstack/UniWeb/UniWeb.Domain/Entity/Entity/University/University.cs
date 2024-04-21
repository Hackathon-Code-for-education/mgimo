using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniWeb.Entities.Entity.Entity.University;

public class University
{
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// Название университета
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Краткое описание университета
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Веб страница университета
    /// </summary>
    public string? Website { get; set; }
    
    /// <summary>
    /// Адрес университета
    /// </summary>
    public string? Address { get; set; }
    
    /// <summary>
    /// Город, где данный университет находится
    /// </summary>
    public string? City { get; set; }
    
    /// <summary>
    /// Тип университета
    /// </summary>
    public string? UniversityType { get; set; }
    
    /// <summary>
    /// Количество программ, проводимых в университете
    /// </summary>
    public int NumbersOfPrograms { get; set; }
    
    /// <summary>
    /// Путь до фотографий об университете
    /// </summary>
    public string? ImagesPath { get; set; }
    
    /// <summary>
    /// Название изображение, которое используется для отображения 
    /// </summary>
    public string? PreviewImageName { get; set; }
    
    /// <summary>
    /// Домен для почты
    /// </summary>
    public string? EmailDomain { get; set; }
    
}