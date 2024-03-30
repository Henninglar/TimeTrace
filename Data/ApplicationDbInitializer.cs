using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeTrace.Models;

namespace TimeTrace.Data;

public class ApplicationDbInitializer
{
    public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
    {
        
        db.Database.EnsureDeleted();

        // Recreate the database and tables according to our models
        db.Database.EnsureCreated();

        // Add test data to simplify debugging and testing

        var adminRole = new IdentityRole("Admin");
        rm.CreateAsync(adminRole).Wait();

        var userRole = new IdentityRole("User");
        rm.CreateAsync(userRole).Wait();
        
        
        var admin = new ApplicationUser{ Orgnr = 1234567, Companyname = "The testing company", UserName = "admin@uia.no", Email = "admin@uia.no", EmailConfirmed = true, firstname = "Admin", lastname = "Adminsen"};
        um.CreateAsync(admin, "Password1.").Wait();
        um.AddToRoleAsync(admin, "Admin");
        
        var user = new ApplicationUser{ Orgnr = 1234567, Companyname = "The testing company", UserName = "user@uia.no", Email = "user@uia.no", EmailConfirmed = true, firstname = "Ole", lastname = "Olsen"};
        user.employeer_id = admin.Id;
        um.CreateAsync(user, "Password1.").Wait();
        um.AddToRoleAsync(user, "User");
        
        var user2 = new ApplicationUser{ firstname = "Andre", lastname = "Hansen", UserName = "user2@uia.no", Email = "user2@uia.no", EmailConfirmed = true};
        user2.employeer_id = admin.Id;
        um.CreateAsync(user2, "Password2.").Wait();
        um.AddToRoleAsync(user2, "User");
        
        var user3 = new ApplicationUser{ firstname = "Henning", lastname = "Hansen", UserName = "user3@uia.no", Email = "user3@uia.no", EmailConfirmed = true};
        user3.employeer_id = admin.Id;
        um.CreateAsync(user3, "Password3.").Wait();
        um.AddToRoleAsync(user3, "User");
        
        var user4 = new ApplicationUser{ firstname = "Ole", lastname = "Hansen", UserName = "user4@uia.no", Email = "user4@uia.no", EmailConfirmed = true};
        user4.employeer_id = admin.Id;
        um.CreateAsync(user4, "Password4.").Wait();
        um.AddToRoleAsync(user4, "User");

        db.SaveChanges();

        var projectOwner = db.Users.ToListAsync().Result[0];

        var project1 = new ProjectModel("TestProject", "Uphill Battle", DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
        project1.ApplicationUsers.Add(projectOwner);
        db.ProjectModel.Add(project1);

        var hours1 = new TimeRegisterModel(5, project1, DateOnly.FromDateTime(DateTime.Today), user2);
        var hours2 = new TimeRegisterModel(15, project1, DateOnly.FromDateTime(DateTime.Today), user3);
        var hours3 = new TimeRegisterModel(35, project1, DateOnly.FromDateTime(DateTime.Today), user4);
        var hours4 = new TimeRegisterModel(8, project1, DateOnly.FromDateTime(DateTime.Today), admin);
        
        db.TimeRegisterModel.Add(hours1);
        db.TimeRegisterModel.Add(hours2);
        db.TimeRegisterModel.Add(hours3);
        db.TimeRegisterModel.Add(hours4);
        db.SaveChanges();
    }
}