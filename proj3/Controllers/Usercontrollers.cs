using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdminLTE.Models;
using poj3.Models;
using System.Text.Json;
using System.Data;

namespace poj3.Controllers
{
    public class UserController : Controller
    {
        private const string CartSessionKey = "ShoppingCart";
        private const string FavoritesSessionKey = "FavoriteProducts";

        // Demo product catalog.
        private static readonly List<Product> Products = new()
        {
            new Product { Id=1, Name="Wireless Audio System Multiroom 360 degree Full base audio", Category="Speakers", Price=299.00m, Image="~/img/212X200/img1.jpg", Description="Premium wireless multiroom audio system with powerful 360-degree sound." },
            new Product { Id=2, Name="Tablet White EliteBook Revolve 810 G2", Category="Tablets", Price=549.00m, Image="~/img/212X200/img2.jpg", Description="Compact and powerful tablet for work, study and entertainment." },
            new Product { Id=3, Name="Purple Solo 2 Wireless", Category="Headphones", Price=199.00m, Image="~/img/212X200/img3.jpg", Description="Comfortable wireless headphones with clear sound and long battery life." },
            new Product { Id=4, Name="Smartphone 6S 32GB LTE", Category="Smartphones", Price=699.00m, Image="~/img/212X200/img4.jpg", Description="Reliable LTE smartphone with 32GB storage and a bright display." },
            new Product { Id=5, Name="Widescreen NX Mini F1 SMART NX", Category="Cameras", Price=685.00m, Image="~/img/212X200/img5.jpg", Description="Compact smart camera with widescreen capture and easy controls." },
            new Product { Id=6, Name="Full Color LaserJet Pro M452dn", Category="Printers", Price=429.00m, Image="~/img/212X200/img6.jpg", Description="Fast full-color laser printer for home and office use." },
            new Product { Id=7, Name="GameConsole Destiny Special Edition", Category="Gaming", Price=499.00m, Image="~/img/212X200/img7.jpg", Description="Special-edition gaming console designed for an immersive experience." },
            new Product { Id=8, Name="Camera C430W 4k Waterproof", Category="Cameras", Price=799.00m, Image="~/img/212X200/img8.jpg", Description="Waterproof 4K camera for travel, sports and outdoor photography." }
        };

        public IActionResult home() => View();
        public IActionResult home2() => View();
        public IActionResult home3() => View();
        public IActionResult home3h() => View();
        public IActionResult home4() => View();
        public IActionResult home5() => View();
        public IActionResult home6() => View();
        public IActionResult home7() => View();
        public IActionResult about() => View();
        public IActionResult ContactV1() => View();
        public IActionResult ContactV2() => View();
        public IActionResult Faq() => View();
        public IActionResult StoreDirectory() => View();
        public IActionResult TermsAndConditions() => View();
        public IActionResult NotFound() => View();

        [HttpGet]
        public IActionResult Product(int id)
        {
            var product = Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return RedirectToAction(nameof(NotFound));
            ViewBag.IsFavorite = GetFavorites().Contains(id);
            return View("Product", product);
        }

        // ================= CART =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            if (quantity < 1) quantity = 1;
            var product = Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.Image,
                    Quantity = Math.Min(quantity, product.Stock)
                });
            }
            else
            {
                item.Quantity = Math.Min(item.Quantity + quantity, product.Stock);
            }

            SaveCart(cart);
            TempData["CartMessage"] = $"{product.Name} added to cart.";
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IncreaseQuantity(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            var product = Products.FirstOrDefault(x => x.Id == id);

            if (item != null && product != null && item.Quantity < product.Stock)
                item.Quantity++;

            SaveCart(cart);
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DecreaseQuantity(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                    cart.Remove(item);
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(int id, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                if (quantity <= 0)
                    cart.Remove(item);
                else
                    item.Quantity = Math.Min(quantity, Products.FirstOrDefault(p => p.Id == id)?.Stock ?? quantity);
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            cart.RemoveAll(x => x.ProductId == id);
            SaveCart(cart);
            return RedirectToAction(nameof(Cart));
        }

        [HttpGet]
        public IActionResult Cart()
        {
            return View("Cart", GetCart());
        }

        [HttpGet]
        public IActionResult BuyNow(int id)
        {
            var product = Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound();

            SaveCart(new List<CartItem>
            {
                new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.Image,
                    Quantity = 1
                }
            });

            return RedirectToAction(nameof(Checkout));
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (cart.Count == 0) return RedirectToAction(nameof(Cart));

            ViewBag.Total = cart.Sum(x => x.Total);
            return View("Checkout", cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(string customerName, string email, string phone, string address)
        {
            var cart = GetCart();
            if (cart.Count == 0) return RedirectToAction(nameof(Cart));

            if (string.IsNullOrWhiteSpace(customerName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(address))
            {
                TempData["CheckoutError"] = "Please fill all customer and delivery details.";
                return RedirectToAction(nameof(Checkout));
            }

            var orderNumber = "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var total = cart.Sum(x => x.Total);

            HttpContext.Session.SetString("LastOrderNumber", orderNumber);
            HttpContext.Session.SetString("LastOrderTotal", total.ToString("0.00"));
            SaveCart(new List<CartItem>());

            return RedirectToAction(nameof(OrderSuccess));
        }

        [HttpGet]
        public IActionResult OrderSuccess()
        {
            ViewBag.OrderNumber = HttpContext.Session.GetString("LastOrderNumber");
            ViewBag.Total = HttpContext.Session.GetString("LastOrderTotal");
            return View("OrderSuccess");
        }

        // ================= FAVORITES =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleFavorite(int id)
        {
            if (Products.All(x => x.Id != id))
                return NotFound();

            var favorites = GetFavorites();

            if (favorites.Contains(id))
            {
                favorites.Remove(id);
                TempData["FavoriteMessage"] = "Product removed from favorites.";
            }
            else
            {
                favorites.Add(id);
                TempData["FavoriteMessage"] = "Product added to favorites.";
            }

            SaveFavorites(favorites);
            return RedirectToAction(nameof(Product), new { id });
        }

        [HttpGet]
        public IActionResult Favorites()
        {
            var favoriteIds = GetFavorites();
            var favoriteProducts = Products
                .Where(p => favoriteIds.Contains(p.Id))
                .ToList();

            return View("Favorites", favoriteProducts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFavorite(int id)
        {
            var favorites = GetFavorites();
            favorites.Remove(id);
            SaveFavorites(favorites);

            return RedirectToAction(nameof(Favorites));
        }

        // ================= MANAGE ACCOUNT =================

        [HttpGet]
        public IActionResult ManageAccount()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Index", "Home");

            var model = new RegisterModel();
            DataSet ds = model.GetById(userId.Value);

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }

            var row = ds.Tables[0].Rows[0];
            model.Id = Convert.ToInt32(row["id"]);
            model.name = Convert.ToString(row["name"]) ?? "";
            model.email = Convert.ToString(row["email"]) ?? "";
            model.password = Convert.ToString(row["password"]) ?? "";

            return View("ManageAccount", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageAccount(RegisterModel model)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Index", "Home");

            if (string.IsNullOrWhiteSpace(model.name) ||
                string.IsNullOrWhiteSpace(model.email) ||
                string.IsNullOrWhiteSpace(model.password))
            {
                ViewBag.msg = "All fields are required.";
                return View("ManageAccount", model);
            }

            if (model.EmailExists(model.email, userId.Value))
            {
                ViewBag.msg = "This email is already registered.";
                return View("ManageAccount", model);
            }

            model.UpdateAccount(userId.Value);

            HttpContext.Session.SetString("UserName", model.name.Trim());
            HttpContext.Session.SetString("UserEmail", model.email.Trim());

            TempData["AccountMessage"] = "Account updated successfully.";
            return RedirectToAction(nameof(ManageAccount));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccount()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Index", "Home");

            var model = new RegisterModel();
            model.DeleteAccount(userId.Value);

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ================= SESSION HELPERS =================

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrWhiteSpace(json))
                return new List<CartItem>();

            try
            {
                return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
            }
            catch
            {
                return new List<CartItem>();
            }
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(
                CartSessionKey,
                JsonSerializer.Serialize(cart));
        }

        private List<int> GetFavorites()
        {
            var json = HttpContext.Session.GetString(FavoritesSessionKey);
            if (string.IsNullOrWhiteSpace(json))
                return new List<int>();

            try
            {
                return JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
            }
            catch
            {
                return new List<int>();
            }
        }

        private void SaveFavorites(List<int> favorites)
        {
            HttpContext.Session.SetString(
                FavoritesSessionKey,
                JsonSerializer.Serialize(favorites.Distinct().ToList()));
        }
    }
}
