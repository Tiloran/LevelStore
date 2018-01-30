using System.Collections.Generic;
using System.Linq;
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

        public ViewResult List(int? categoryID, int? subCategoryID)
        {
            List<ProductWithImages> productAndImages = new List<ProductWithImages>();
            List<Product> products = new List<Product>();
            List<Image> images = new List<Image>();
            if (subCategoryID != null)
            {
                products = new List<Product>(repository.Products.Where(pSCId => pSCId.SubCategoryID == subCategoryID).Where(h => h.HideFromUsers == false).OrderBy(pId => pId.ProductID));
            }
            else if (categoryID != null)
            {
                List<SubCategory> subCategories =
                    new List<SubCategory>(repository.SubCategories.Where(i => i.CategoryID == categoryID).ToList());
                repository.SubCategories.Where(i => i.CategoryID == categoryID);
                products = new List<Product>(repository.Products
                    .Where(pSCId => subCategories.Any(sCId => pSCId.SubCategoryID == sCId.SubCategoryID))
                    .Where(h => h.HideFromUsers == false)
                    .OrderBy(pId => pId.ProductID));
            }
            else
            {
                products = new List<Product>(repository.Products.Where(h => h.HideFromUsers == false).OrderBy(p => p.ProductID));
            }
            images = new List<Image>(repository.Images);

            for (int i = 0; i < products.Count; i++)
            {
                images = new List<Image>(repository.Images.Where(index => index.ProductID == products[i].ProductID));
                productAndImages.Add(new ProductWithImages()
                {
                    product = products[i],
                    Images = images
                });
            }
            List<Category> categories = repository.GetCategoriesWithSubCategories();

            ProductsListViewModel productsListViewModel = new ProductsListViewModel { ProductAndImages = productAndImages.ToList(), Categories = categories};
            return View(productsListViewModel);
        }

        

        [HttpPost]
        public ViewResult ViewSingleProduct(int productId)
        {
            Product selectedProduct = repository.Products.Where(h => h.HideFromUsers == false).FirstOrDefault(p => p.ProductID == productId);
            if (selectedProduct == null)
            {
                return View("List");
            }
            List<Image> productImages = repository.Images.Where(p => p.ProductID == productId).ToList();
            selectedProduct.Images = productImages;
            var subCategory = 
                repository.SubCategories.First(sCId => sCId.SubCategoryID == selectedProduct.SubCategoryID);
            TempData["Category"] = repository.Categories.First(cId => cId.CategoryID == subCategory.CategoryID).CategoryName;
            TempData["SubCategory"] = subCategory.SubCategoryName;
            return View(selectedProduct);
        }
    }
}