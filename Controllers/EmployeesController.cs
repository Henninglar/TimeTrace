using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrace.Data;
using TimeTrace.Models;

namespace TimeTrace.Controllers;

public class EmployeesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _um;

    public EmployeesController(ApplicationDbContext context, UserManager<ApplicationUser> um)
    {
        _context = context;
        _um = um;
    }
    // GET
    [Authorize(Roles = "Admin")]
    public IActionResult Index()

    {
        var employerId =_um.GetUserAsync(User).Result.Id;
        var result = _context.ApplicationUsers
            .Where(u => u.employeer_id == employerId)
            .ToList();
        return View(result);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var user2delete = await _um.FindByIdAsync(id);
        if (user2delete != null)
        {
            
            _context.ApplicationUsers.Remove(user2delete);
            var timelists = _context.TimeRegisterModel.Where(u => u.User == user2delete).ToList();
            foreach (var t in timelists)
            {
                _context.Remove(t);
            }
            
            await _context.SaveChangesAsync();
            
            IdentityResult result = await _um.DeleteAsync(user2delete);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));  
            }
        }
        else
            ModelState.AddModelError("", "User Not Found");
        return RedirectToAction(nameof(Index));
    }
}