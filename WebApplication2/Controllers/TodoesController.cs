using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using WebApplication2.Data;
using WebApplication2.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace WebApplication2.Controllers
{
    public class TodoesController : Controller
    {
        private readonly WebApplication2Context _context;

        public TodoesController(WebApplication2Context context)
        {
            _context = context;
        }

        // GET: Todoes
        public async Task<IActionResult> Index()
        {
            return _context.Todo != null ?
                        View(await _context.Todo.ToListAsync()) :
                        Problem("Entity set 'WebApplication2Context.Todo'  is null.");
        }

        [HttpPost("Todoes")]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody]Todo todo)
        {
            if (_context.Todo == null)
            {
                return Problem("Entity set 'WebApplication2Context.Todo' is null.");
            }
            if (todo.Title.Length < 1)
            {
                return BadRequest(new { message = "Invalid data" });
            }
            todo.CreatedAt = DateTime.Now;
            _context.Add(todo);
            await _context.SaveChangesAsync();
            return Json(todo);
        }

        // GET: Todoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Todo == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // GET: Todoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,IsDeleted,IsCompleted,CreatedAt,CompletedAt")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Todo == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        // POST: Todoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IsDeleted,IsCompleted,CreatedAt,CompletedAt")] Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
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
            return View(todo);
        }

        // GET: Todoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Todo == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Todo == null)
            {
                return Problem("Entity set 'WebApplication2Context.Todo'  is null.");
            }
            var todo = await _context.Todo.FindAsync(id);
            if (todo != null)
            {
                _context.Todo.Remove(todo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/Todoes/MarkAsComplete/{id}")]
        public async Task<IActionResult> MarkAsComplete(int id)
        {
            var todo = await _context.Todo.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            if (todo.IsCompleted)
            {
                return BadRequest(new { message = "Already updated!" });
            }

            todo.IsCompleted = true;
            todo.CompletedAt = DateTime.Now;

            _context.Update(todo);
            await _context.SaveChangesAsync();

            return Content("success");
        }

        private bool TodoExists(int id)
        {
          return (_context.Todo?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
