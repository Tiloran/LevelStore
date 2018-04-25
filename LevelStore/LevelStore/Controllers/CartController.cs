using System.Linq;
using LevelStore.Infrastructure.ModelState;
using LevelStore.Models;
using LevelStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using NPOI.SS.Formula.PTG;

namespace LevelStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IShareRepository _shareRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly Cart _cart;

        public CartController(IProductRepository repo, Cart cartService, IShareRepository shareRepo, IOrderRepository orderRepo)
        {
            _repository = repo;
            _shareRepository = shareRepo;
            _cart = cartService;
            _orderRepository = orderRepo;
        }

        [ImportModelState]
        public ViewResult Index()
        {
            foreach (var line in _cart.Lines)
            {
                line.Product.Images = _repository.Images.Where(i => i.ProductID == line.Product.ProductID).ToList();
            }

            Order order;
            var value = TempData["order"];
            if (value is string json)
            {
                order = JsonConvert.DeserializeObject<Order>(json);
            }
            else
            {
                order = new Order();
            }

            TempData["colors"] = _repository.TypeColors.ToList();
            TempData["Shares"] = _shareRepository.Shares.ToList();
            CartWithOrderViewModel cartWithOrder = new CartWithOrderViewModel {Cart = _cart, Order = order };
            return View("ListCart", cartWithOrder);
        }

        public IActionResult SetPromoCode(string prmCode)
        {
            Promo promo = _repository.PromoCodes.FirstOrDefault(c => c.PromoCode == prmCode);
            if (promo != null)
            {
                _cart.SetPromoCode(promo.Discount, promo.PromoCode);
                return RedirectToActionPermanent("Index");
            }

            return RedirectToActionPermanent("DeletePromoCode");
        }

        public IActionResult DeletePromoCode(string check)
        {
            _cart.DeletePromoCode();
            return RedirectToActionPermanent("Index");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ExportModelState]
        public IActionResult Checkout(Order order)
        {
            if (!_cart.Lines.Any())
            {
                ModelState.AddModelError("", "Ваша корзина пуста!");
                TempData["order"] = JsonConvert.SerializeObject(order);
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray();
                int CodeDiscount = _cart.PromoCodeDiscount;
                foreach (var line in order.Lines)
                {
                    if (line.Product.ShareID != null || CodeDiscount > 0)
                    {
                        _repository.AddBuyCount(line.Product.ProductID);
                        if (CodeDiscount == 0)
                        {
                            Share share = _shareRepository.Shares.First(i => i.ShareId == line.Product.ShareID);
                            if (share.Enabled)
                            {
                                line.KoefPriceAfterCheckout = share.KoefPrice;
                                line.FakeShare = share.Fake;
                            }
                        }
                        else
                        {
                            line.KoefPriceAfterCheckout = CodeDiscount;
                            line.FakeShare = false;
                        }
                    }
                    line.PriceAfterCheckout = line.Product.Price;
                }
                _orderRepository.SaveOrder(order);
                return RedirectToAction("Completed", "Order");
            }
            TempData["order"] = JsonConvert.SerializeObject(order);
            return RedirectToAction("Index");
        }

        public IActionResult AddToCart(int productId, int quantity, int? furniture, int? selectedColor)
        {
            if (furniture == null || selectedColor == null || selectedColor == 0)
            {
                return RedirectToAction($"ViewSingleProduct", new RouteValueDictionary(
                    new
                    {
                        controller = "Product",
                        action = "ViewSingleProduct",
                        productId = productId,
                        wasError = true
                    }));
            }
            if (quantity == 0)
            {
                quantity = 1;
            }
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                _cart.AddItem(product, quantity, (int) furniture, (int) selectedColor);
                _repository.AddAnAddOnCountToTheCart(product.ProductID);
            }
            return RedirectToAction("List", "Product");
        }

        public IActionResult IncreaseQuantity(int productId)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                _cart.IncreaseQuantity(productId);
            }
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult DecreaseQuantity(int productId)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                _cart.DecreaseQuantity(productId);
            }
            return RedirectToAction("Index", "Cart");
        }

        public RedirectToActionResult RemoveFromCart(int productId)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                _cart.RemoveLine(product);
            }
            return RedirectToAction("List", "Product");
        }
    }
}