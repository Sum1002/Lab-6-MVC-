using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookshopManagement.Data;
using BookshopManagement.Models;

namespace BookshopManagement.Controllers
{
    public class CustomersController : Controller
    {
        private readonly BookshopContext _context;

        public CustomersController(BookshopContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.BookShop)
                .ThenInclude(bs => bs.Book)
                .Include(c => c.Orders)
                .ThenInclude(o => o.BookShop)
                .ThenInclude(bs => bs.Shop)
                .FirstOrDefaultAsync(m => m.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            // Get shops for dropdown
            var shops = await _context.Shops.ToListAsync();
            ViewData["Shops"] = new SelectList(shops, "ShopId", "Name");
            ViewData["CustomerId"] = id;

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Phone,Address,City,State,PostalCode,Country,DateOfBirth")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Email,Phone,Address,City,State,PostalCode,Country,DateOfBirth")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/GetBooksForShop
        [HttpGet]
        public async Task<IActionResult> GetBooksForShop(int shopId)
        {
            var books = await _context.BookShops
                .Where(bs => bs.ShopId == shopId && bs.Quantity > 0)
                .Include(bs => bs.Book)
                .Select(bs => new { 
                    BookShopId = bs.BookShopId, 
                    BookTitle = bs.Book.Title,
                    Author = bs.Book.Author,
                    AvailableQuantity = bs.Quantity,
                    Price = bs.ShopPrice ?? bs.Book.Price
                })
                .ToListAsync();

            return Json(books);
        }

        // POST: Customers/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(int customerId, int bookShopId, int quantity)
        {
            try
            {
                // Get the BookShop entry to check availability
                var bookShop = await _context.BookShops
                    .Include(bs => bs.Book)
                    .Include(bs => bs.Shop)
                    .FirstOrDefaultAsync(bs => bs.BookShopId == bookShopId);

                if (bookShop == null)
                {
                    ModelState.AddModelError("", "Selected book is no longer available.");
                    return RedirectToAction(nameof(Details), new { id = customerId });
                }

                // Validate quantity
                if (quantity <= 0)
                {
                    ModelState.AddModelError("", "Quantity must be greater than 0.");
                    return RedirectToAction(nameof(Details), new { id = customerId });
                }

                if (quantity > bookShop.Quantity)
                {
                    ModelState.AddModelError("", $"Only {bookShop.Quantity} copies of '{bookShop.Book.Title}' are available in {bookShop.Shop.Name}.");
                    return RedirectToAction(nameof(Details), new { id = customerId });
                }

                // Calculate total price
                var unitPrice = bookShop.ShopPrice ?? bookShop.Book.Price;
                var totalPrice = unitPrice * quantity;

                // Create the order
                var order = new Order
                {
                    CustomerId = customerId,
                    BookShopId = bookShopId,
                    Quantity = quantity,
                    OrderDate = DateTime.Now,
                    TotalPrice = totalPrice,
                    OrderStatus = "Pending"
                };

                // Add order to database
                _context.Orders.Add(order);

                // Reduce the quantity in BookShop
                bookShop.Quantity -= quantity;

                // Save changes
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Order placed successfully! Total: {totalPrice:C}";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while placing the order. Please try again.");
            }

            return RedirectToAction(nameof(Details), new { id = customerId });
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
