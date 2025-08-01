using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Controllers
{
    public class TaskListController : Controller
    {
        private readonly AppDbContext _context;

        public TaskListController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TaskList
        public async Task<IActionResult> Index()
        {
            return View(await _context.TaskLists.ToListAsync());
        }

        // GET: TaskList/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskList = await _context.TaskLists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskList == null)
            {
                return NotFound();
            }

            return View(taskList);
        }

        // GET: TaskList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CreatedAt")] TaskList taskList)
        {
            if (ModelState.IsValid)
            {
                taskList.CreatedAt = taskList.CreatedAt.ToUniversalTime();
                taskList.Id = Guid.NewGuid();
                _context.Add(taskList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskList);
        }

        // GET: TaskList/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskList = await _context.TaskLists.FindAsync(id);
            if (taskList == null)
            {
                return NotFound();
            }
            return View(taskList);
        }

        // POST: TaskList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,CreatedAt")] TaskList taskList)
        {
            if (id != taskList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    taskList.CreatedAt = taskList.CreatedAt.ToUniversalTime();
                    _context.Update(taskList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskListExists(taskList.Id))
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
            return View(taskList);
        }

        // GET: TaskList/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskList = await _context.TaskLists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskList == null)
            {
                return NotFound();
            }

            return View(taskList);
        }

        // POST: TaskList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var taskList = await _context.TaskLists.FindAsync(id);
            if (taskList != null)
            {
                _context.TaskLists.Remove(taskList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskListExists(Guid id)
        {
            return _context.TaskLists.Any(e => e.Id == id);
        }
    }
}
