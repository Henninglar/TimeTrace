using Microsoft.AspNetCore.Identity;

namespace TimeTrace.Models;

public class ApplicationUser : IdentityUser
{

    public ApplicationUser()
    {
        Projects = new List<ProjectModel>();
    }
    public string? Companyname { get; set; }  = string.Empty;
    
    public string? firstname { get; set; }  = string.Empty;
    
    public string? lastname { get; set; }  = string.Empty;
    public int? Orgnr { get; set; }

    public virtual ICollection<ProjectModel> Projects { get; set; }
    
    public string? employeer_id { get; set; }

}