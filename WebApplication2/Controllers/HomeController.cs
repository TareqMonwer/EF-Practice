using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Schema;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly WebApplication2Context _context;

        public HomeController(ILogger<HomeController> logger, WebApplication2Context context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var todoes = _context.Todo.AsNoTracking().Where(todo => !todo.IsDeleted);
            return View(await PaginatedList<Todo>.CreateAsync(todoes.AsNoTracking(), pageNumber ?? 1));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}