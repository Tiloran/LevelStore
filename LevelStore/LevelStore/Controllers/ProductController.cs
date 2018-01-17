using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelStore.Models;
using LevelStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category) =>
            View(new ProductsListViewModel
            {
                Product = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID),
                CurrentCategory = category
            });

    }
}