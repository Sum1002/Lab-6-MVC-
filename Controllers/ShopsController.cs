using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            // Get available books (not already in this shop)
            var availableBooks = await _context.Books
                .Where(b => !b.BookShops.Any(bs => bs.ShopId == id))
                .ToListAsync();

            ViewData["AvailableBooks"] = new SelectList(availableBooks, "BookId", "Title");
            ViewData["ShopId"] = id;

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

        // POST: Shops/AddBook
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(int shopId, int bookId, int quantity, decimal? shopPrice)
        {
            if (ModelState.IsValid)
            {
                var bookShop = new BookShop
                {
                    ShopId = shopId,
                    BookId = bookId,
                    Quantity = quantity,
                    ShopPrice = shopPrice
                };

                _context.Add(bookShop);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = shopId });
        }

        // GET: Shops/EditBookShop/5
        public async Task<IActionResult> EditBookShop(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookShop = await _context.BookShops
                .Include(bs => bs.Book)
                .Include(bs => bs.Shop)
                .FirstOrDefaultAsync(bs => bs.BookShopId == id);

            if (bookShop == null)
            {
                return NotFound();
            }

            return View(bookShop);
        }

        // POST: Shops/EditBookShop/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBookShop(int id, [Bind("BookShopId,ShopId,BookId,Quantity,ShopPrice,Notes")] BookShop bookShop)
        {
            if (id != bookShop.BookShopId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookShop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookShopExists(bookShop.BookShopId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = bookShop.ShopId });
            }

            return View(bookShop);
        }

        // GET: Shops/DeleteBookShop/5
        public async Task<IActionResult> DeleteBookShop(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookShop = await _context.BookShops
                .Include(bs => bs.Book)
                .Include(bs => bs.Shop)
                .FirstOrDefaultAsync(bs => bs.BookShopId == id);

            if (bookShop == null)
            {
                return NotFound();
            }

            return View(bookShop);
        }

        // POST: Shops/DeleteBookShop/5
        [HttpPost, ActionName("DeleteBookShop")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBookShopConfirmed(int id)
        {
            var bookShop = await _context.BookShops.FindAsync(id);
            if (bookShop != null)
            {
                var shopId = bookShop.ShopId;
                _context.BookShops.Remove(bookShop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = shopId });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ShopExists(int id)
        {
            return _context.Shops.Any(e => e.ShopId == id);
        }

        private bool BookShopExists(int id)
        {
            return _context.BookShops.Any(e => e.BookShopId == id);
        }
    }
}
