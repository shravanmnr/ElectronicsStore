using System;
using System.Linq;
using System.Web.Mvc;
using ElectronicsStore.Models;
using ElectronicsStore.Core.Application.Services;

namespace ElectronicsStore.Controllers
{
    [RoutePrefix("cart")]
    public class CartController : BaseController
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        private Cart GetCart() => Session["Cart"] as Cart ?? new Cart();
        private void SaveCart(Cart cart) => Session["Cart"] = cart;

        // GET /cart
        [HttpGet]
        [Route("")]
        public ActionResult Index() => OkView(GetCart());

        // POST /cart/add/{productId}
        [HttpPost]
        [Route("add/{productId:int}")]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int productId, int quantity = 1, string returnUrl = null)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                return HttpNotFound($"Product {productId} not found.");

            var cart = GetCart();
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
                existing.Quantity += quantity;
            else
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });

            SaveCart(cart);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index");
        }

        // POST /cart/update-item  (handles both quantity update and remove)
        [HttpPost]
        [Route("update-item")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateItem(int productId, int quantity, string action)
        {
            var cart = GetCart();
            if (action == "remove")
            {
                cart.Items.RemoveAll(i => i.ProductId == productId);
            }
            else
            {
                var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                    item.Quantity = Math.Max(1, quantity);
            }
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // POST /cart/clear
        [HttpPost]
        [Route("clear")]
        [ValidateAntiForgeryToken]
        public ActionResult Clear()
        {
            SaveCart(new Cart());
            return RedirectToAction("Index");
        }
    }
}
