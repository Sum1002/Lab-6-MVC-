using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookshopManagement.Data;
using BookshopManagement.Models;

namespace BookshopManagement.Controllers
{
    public class ShopsController : Controller
    {
        private readonly BookshopContext _context;

        public ShopsController(BookshopContext context)
        {
            _context = context;
        }

        // GET: Shops
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shops.ToListAsync());
        }

        // GET: Shops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .Include(s => s.BookShops)
                .ThenInclude(bs => bs.Book)
                .FirstOrDefaultAsync(m => m.ShopId == id);

            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // GET: Shops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shops/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Location,Address,Phone,Email,Website,OpeningYear")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shop);
        }

        // GET: Shops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }
            return View(shop);
        }

        // POST: Shops/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShopId,Name,Location,Address,Phone,Email,Website,OpeningYear")] Shop shop)
        {
            if (id != shop.ShopId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopExists(shop.ShopId))
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
            return View(shop);
        }

        // GET: Shops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .FirstOrDefaultAsync(m => m.ShopId == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop != null)
            {
                _context.Shops.Remove(shop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopExists(int id)
        {
            return _context.Shops.Any(e => e.ShopId == id);
        }
    }
}
