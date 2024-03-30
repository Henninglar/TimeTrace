using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TimeTrace.Data;
using TimeTrace.Models;

namespace TimeTrace.Controllers
{
    public class TimeRegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _um;

        public TimeRegisterController(ApplicationDbContext context, UserManager<ApplicationUser> um)
        {
            _context = context;
            _um = um;
        }

        // GET: TimeRegister
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var employee =_um.GetUserAsync(User).Result;

            var result = _context.TimeRegisterModel.
                Where(u => u.User == employee).
                Include(p => p.Project)
                .ToList();
            return View(result);
        }

        // GET: TimeRegister/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TimeRegisterModel == null)
            {
                return NotFound();
            }

            var timeRegisterModel = await _context.TimeRegisterModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeRegisterModel == null)
            {
                return NotFound();
            }

            return View(timeRegisterModel);
        }

        // GET: TimeRegister/Create
        [Authorize]
        public IActionResult Create()
        {
            var user = _um.GetUserAsync(User).Result;
            var employee = _context.ApplicationUsers.Include(usr => usr.Projects).Single(u => u == user);

            ViewBag.Projects = new List<ProjectModel>();
            foreach (var project in employee.Projects)
            {
                ViewBag.Projects.Add(project);
            }
            
            
            return View();
        }

        // POST: TimeRegister/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Hours,ProjectId,Date")] TimeRegisterModel timeRegisterModel)
        {
            
            
            if (ModelState.IsValid)
            {
                var employee =_um.GetUserAsync(User);
                timeRegisterModel.User = employee.Result;

                var project = await _context.ProjectModel.FindAsync(timeRegisterModel.ProjectId);
                timeRegisterModel.Project = project;
                
                _context.Add(timeRegisterModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Projects = new List<ProjectModel>();
            return View(timeRegisterModel);
        }

        // GET: TimeRegister/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TimeRegisterModel == null)
            {
                return NotFound();
            }

            var timeRegisterModel = await _context.TimeRegisterModel.FindAsync(id);
            if (timeRegisterModel == null)
            {
                return NotFound();
            }
            return View(timeRegisterModel);
        }

        // POST: TimeRegister/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Hours,Date,Project,User")] TimeRegisterModel timeRegisterModel)
        {
            if (id != timeRegisterModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeRegisterModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeRegisterModelExists(timeRegisterModel.Id))
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
            return View(timeRegisterModel);
        }

        // GET: TimeRegister/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TimeRegisterModel == null)
            {
                return NotFound();
            }

            var timeRegisterModel = await _context.TimeRegisterModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeRegisterModel == null)
            {
                return NotFound();
            }

            return View(timeRegisterModel);
        }

        // POST: TimeRegister/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TimeRegisterModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TimeRegisterModel'  is null.");
            }
            var timeRegisterModel = await _context.TimeRegisterModel.FindAsync(id);
            if (timeRegisterModel != null)
            {
                _context.TimeRegisterModel.Remove(timeRegisterModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeRegisterModelExists(int id)
        {
          return _context.TimeRegisterModel.Any(e => e.Id == id);
        }
    }
}
