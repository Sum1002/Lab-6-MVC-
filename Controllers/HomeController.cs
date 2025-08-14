using Microsoft.AspNetCore.Mvc;
using BookshopManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace BookshopManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookshopContext _context;

        public HomeController(BookshopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = new
            {
                TotalBooks = await _context.Books.CountAsync(),
                TotalShops = await _context.Shops.CountAsync(),
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                RecentOrders = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.BookShop)
                    .ThenInclude(bs => bs.Book)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboardData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
