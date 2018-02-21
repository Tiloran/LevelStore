using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelStore.Models;
using LevelStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly IProductRepository repository;

        public CategoriesViewComponent(IProductRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            IEnumerable<Category> categories = repository.GetCategoriesWithSubCategories();

            return View("CategoriesAdmin",categories);
        }
    }
}
