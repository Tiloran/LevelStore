using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private Cart cart;

        public CartController(IProductRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }

        public ViewResult Index()
        {
            return View("ListCart", cart);
        }

        public RedirectToActionResult AddToCart(int productId)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("List", "Product");
        }

        public RedirectToActionResult RemoveFromCart(int productId)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("List", "Product");
        }
    }
}