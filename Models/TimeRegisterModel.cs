using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace TimeTrace.Models;

public class TimeRegisterModel
{
    public TimeRegisterModel() {}
        
    public TimeRegisterModel(float hours, ProjectModel project, DateOnly time, ApplicationUser applicationUser)
    {
        Hours = hours;
        Project = project;
        User = applicationUser;
        Date = time;
    }
    
    public TimeRegisterModel(float hours, int projectId, DateOnly time)
    {
        Hours = hours;
        ProjectId = projectId;
        Date = time;
    }
    
    public int Id { get; set; }
    
    [Required]
    [DisplayName("Hours")]
    public float Hours { get; set; }
    
    public ProjectModel? Project { get; set; }
    
    [Required]
    [DisplayName("Project")]
    public int ProjectId { get; set; }
    public ApplicationUser? User { get; set; }

    [DataType(DataType.Date)]
    public DateOnly Date { get; set; }

}