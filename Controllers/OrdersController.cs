using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookshopManagement.Data;
using BookshopManagement.Models;

namespace BookshopManagement.Controllers
{
    public class OrdersController : Controller
    {
        private readonly BookshopContext _context;

        public OrdersController(BookshopContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.BookShop)
                .ThenInclude(bs => bs.Book)
                .Include(o => o.BookShop)
                .ThenInclude(bs => bs.Shop)
                .ToListAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.BookShop)
                .ThenInclude(bs => bs.Book)
                .Include(o => o.BookShop)
                .ThenInclude(bs => bs.Shop)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName");
            ViewData["BookShopId"] = new SelectList(_context.BookShops
                .Include(bs => bs.Book)
                .Include(bs => bs.Shop)
                .Select(bs => new { bs.BookShopId, DisplayText = $"{bs.Book.Title} - {bs.Shop.Name}" }), "BookShopId", "DisplayText");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,BookShopId,Quantity,TotalPrice,OrderStatus,OrderNotes,ShippingMethod,ShippingCost")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["BookShopId"] = new SelectList(_context.BookShops
                .Include(bs => bs.Book)
                .Include(bs => bs.Shop)
                .Select(bs => new { bs.BookShopId, DisplayText = $"{bs.Book.Title} - {bs.Shop.Name}" }), "BookShopId", "DisplayText", order.BookShopId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["BookShopId"] = new SelectList(_context.BookShops
                .Include(bs => bs.Book)
                .Include(bs => bs.Shop)
                .Select(bs => new { bs.BookShopId, DisplayText = $"{bs.Book.Title} - {bs.Shop.Name}" }), "BookShopId", "DisplayText", order.BookShopId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,BookShopId,Quantity,OrderDate,TotalPrice,OrderStatus,OrderNotes,ShippedDate,ShippingMethod,ShippingCost")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["BookShopId"] = new SelectList(_context.BookShops
                .Include(bs => bs.Book)
                .Include(bs => bs.Shop)
                .Select(bs => new { bs.BookShopId, DisplayText = $"{bs.Book.Title} - {bs.Shop.Name}" }), "BookShopId", "DisplayText", order.BookShopId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.BookShop)
                .ThenInclude(bs => bs.Book)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
