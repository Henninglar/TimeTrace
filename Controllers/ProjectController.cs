using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TimeTrace.Data;
using TimeTrace.Models;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace TimeTrace.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _um;

        public ProjectController(ApplicationDbContext context, UserManager<ApplicationUser> um)
        {
            _context = context;
            _um = um;
        }

        // GET: Project
        [Authorize]
        public async Task<IActionResult> Index()

        {
            var employee = await _um.GetUserAsync(User);
            
            //var result = _context.ProjectModel.FromSqlRaw("SELECT * FROM ProjectModel WHERE EmployeeID = '" +employee+"'")
            //    .ToList();

            var result = new List<ProjectModel>();
            var Projects = await _context.ProjectModel
                .Include(p => p.ApplicationUsers)
                .ToListAsync();
            foreach (var project in Projects)
            {
                if (project.ApplicationUsers.Contains(employee))
                {
                    result.Add(project);
                }
            }
            return View(result);
        }
        

        // GET: Project/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProjectModel == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // GET: Project/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create( ProjectModel projectModel)
        {
            var employee = _um.GetUserAsync(User).Result;
            projectModel.ApplicationUsers = new List<ApplicationUser>() {employee};
            if (ModelState.IsValid)
            {
                _context.Add(projectModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectModel);
        }
        
        // GET: Project/Assign/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assign(int? id)
        {
            if (id == null || _context.ProjectModel == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModel
                .Include(p => p.ApplicationUsers)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }
            return View(projectModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assign(int id, ProjectModel projectModel)
        {

            var userToAdd = _um.FindByEmailAsync(projectModel.UsernameToAdd).Result;
            var project = await _context.ProjectModel
                .Include(p => p.ApplicationUsers)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null || userToAdd == null)
            {
                return NotFound();
            }
            project.ApplicationUsers.Add(userToAdd);

            _context.Update(project);
            await _context.SaveChangesAsync();
            
            return View(project);
        }

        // GET: Project/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProjectModel == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModel.FindAsync(id);
            if (projectModel == null)
            {
                return NotFound();
            }
            return View(projectModel);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,EmployeeID,Start,End")] ProjectModel projectModel)
        {
            if (id != projectModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectModelExists(projectModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(projectModel);
        }

        // GET: Project/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProjectModel == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProjectModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProjectModel'  is null.");
            }
            var projectModel = await _context.ProjectModel.FindAsync(id);
            if (projectModel != null)
            {
                _context.ProjectModel.Remove(projectModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectModelExists(int id)
        {
          return _context.ProjectModel.Any(e => e.Id == id);
        }
    }
}
