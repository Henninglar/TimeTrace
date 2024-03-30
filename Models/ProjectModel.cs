using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TimeTrace.Models;

public class ProjectModel
{
    public ProjectModel() {}
        
    public ProjectModel(string name, string description, DateOnly start, DateOnly end)
    {
        Name = name;
        Description = description;
        Start = start;
        End = end;
        ApplicationUsers = new List<ApplicationUser>();
        UsernameToAdd = string.Empty;
    }

    public ProjectModel(string usernameToAdd)
    {
        UsernameToAdd = usernameToAdd;
    }
    
    
    public int Id { get; set; }
    
    [Required]
    [DisplayName("Project name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [DisplayName("Project description")]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;
    
    [DisplayName("Assigned workers")]
    public ICollection<ApplicationUser>? ApplicationUsers { get; set; }

    public string? UsernameToAdd { get; set; } = string.Empty;

    //public string OwnerId { get; set; } = string.Empty;
    
    [DataType(DataType.Date)]
    public DateOnly Start { get; set; }
    
    [DataType(DataType.Date)]
    public DateOnly End { get; set; }
    

}