using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using TimeTrace.Data;
using TimeTrace.Models;

namespace TimeTrace.Controllers;

public class ExportController : Controller
{
    
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _um;

    public ExportController(ApplicationDbContext context, UserManager<ApplicationUser> um)
    {
        _context = context;
        _um = um;
    }
    // GET
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
   
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult ExportCSV(Export export)
    {
        var projectname = export.ProjectToSearch;
        var start = export.From;
        var end = export.To;
        

        
        var result = _context.TimeRegisterModel.Include(u => u.User)
            .Where(u => projectname == u.Project.Name)
            .Where(u => start <= u.Date).Where(u => end >= u.Date).ToList();
        var builder = new StringBuilder();
        builder.AppendLine("Employee,Hours,Name,Date");
        foreach (var user in result)
        {
            builder.AppendLine($"{user.User.firstname},{user.Hours},{projectname},{user.Date}");
        }
        return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "user.csv");
    }
}