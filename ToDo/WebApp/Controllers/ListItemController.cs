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
    public class ListItemController : Controller
    {
        private readonly AppDbContext _context;

        public ListItemController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ListItem
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ListItems.Include(l => l.ParentItem).Include(l => l.TaskList);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ListItem/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listItem = await _context.ListItems
                .Include(l => l.ParentItem)
                .Include(l => l.TaskList)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listItem == null)
            {
                return NotFound();
            }

            return View(listItem);
        }

        // GET: ListItem/Create
        public IActionResult Create()
        {
            ViewData["ParentItemId"] = new SelectList(_context.ListItems, "Id", "Description");
            ViewData["TaskListId"] = new SelectList(_context.TaskLists, "Id", "Title");
            return View();
        }

        // POST: ListItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,IsDone,Priority,CreatedAt,DueAt,TaskListId,ParentItemId")] ListItem listItem)
        {
            if (ModelState.IsValid)
            {
                listItem.CreatedAt = listItem.CreatedAt.ToUniversalTime();
                listItem.DueAt = listItem.DueAt?.ToUniversalTime();
                listItem.Id = Guid.NewGuid();
                _context.Add(listItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentItemId"] = new SelectList(_context.ListItems, "Id", "Description", listItem.ParentItemId);
            ViewData["TaskListId"] = new SelectList(_context.TaskLists, "Id", "Title", listItem.TaskListId);
            return View(listItem);
        }

        // GET: ListItem/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listItem = await _context.ListItems.FindAsync(id);
            if (listItem == null)
            {
                return NotFound();
            }
            ViewData["ParentItemId"] = new SelectList(_context.ListItems, "Id", "Description", listItem.ParentItemId);
            ViewData["TaskListId"] = new SelectList(_context.TaskLists, "Id", "Title", listItem.TaskListId);
            return View(listItem);
        }

        // POST: ListItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Description,IsDone,Priority,CreatedAt,DueAt,TaskListId,ParentItemId")] ListItem listItem)
        {
            if (id != listItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    listItem.CreatedAt = listItem.CreatedAt.ToUniversalTime();
                    listItem.DueAt = listItem.DueAt?.ToUniversalTime();
                    _context.Update(listItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListItemExists(listItem.Id))
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
            ViewData["ParentItemId"] = new SelectList(_context.ListItems, "Id", "Description", listItem.ParentItemId);
            ViewData["TaskListId"] = new SelectList(_context.TaskLists, "Id", "Title", listItem.TaskListId);
            return View(listItem);
        }

        // GET: ListItem/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listItem = await _context.ListItems
                .Include(l => l.ParentItem)
                .Include(l => l.TaskList)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listItem == null)
            {
                return NotFound();
            }

            return View(listItem);
        }

        // POST: ListItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var listItem = await _context.ListItems.FindAsync(id);
            if (listItem != null)
            {
                _context.ListItems.Remove(listItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListItemExists(Guid id)
        {
            return _context.ListItems.Any(e => e.Id == id);
        }
    }
}
