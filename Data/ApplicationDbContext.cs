using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeTrace.Models;

namespace TimeTrace.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<TimeTrace.Models.ProjectModel> ProjectModel { get; set; }
    public DbSet<TimeTrace.Models.TimeRegisterModel> TimeRegisterModel { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

}

