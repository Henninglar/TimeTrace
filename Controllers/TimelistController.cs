using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using TimeTrace.Data;
using TimeTrace.Models;

namespace TimeTrace.Controllers;

public class Timelist : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _um;

    public Timelist(ApplicationDbContext context, UserManager<ApplicationUser> um)
    {
        _context = context;
        _um = um;
    }
    // GET
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index(string SortOrder, string SearchString)
    {
        
        var user = _um.GetUserAsync(User).Result.Id;
        var result = from b in _context.TimeRegisterModel.Include(u => u.User)
            select b;

        ViewData["ProjectSortParam"] = String.IsNullOrEmpty(SortOrder) ? "project_sort" : "";
        ViewData["UserSortParam"] = String.IsNullOrEmpty(SortOrder) ? "user_sort" : "";
        ViewData["CurrentFilter"] = SearchString;

        if (!String.IsNullOrEmpty(SearchString))
        {
            result = result.Where(b => b.Project.Name.Contains(SearchString));
        }
        

        switch (SortOrder)
        {
            case "project_sort":
                result = result.OrderBy(b => b.Project.Name);
                break;
            case "user_sort":
                result = result.OrderBy(b => b.User.Email);
                break;
        }

        var test = _um.GetUserAsync(User).Result.Id;
        return View(await result.AsNoTracking().Include(p => p.Project).Include(k => k.User).Where(k => k.User.employeer_id == test).ToListAsync());
    }
}