using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Create() => View("Edit", new Product());

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                //TempData["message"] = $"{product.Name} has been saved";
                return RedirectToAction(controllerName: "Product", actionName: "List");
            }
            else
            {
                //Some error
                return View();
            }
        }
    }
}