using System;
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

        public ViewResult List(int? categoryID, int? subCategoryID, string searchString)
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

            List<Category> categories = repository.GetCategoriesWithSubCategories();

            if (!string.IsNullOrEmpty(searchString))
            {
                List<Product> searchByNameList = new List<Product>(products.Where(n => n.Name.Contains(searchString)));
                List<Product> searchByDescriptionList = new List<Product>(products.Where(d => d.Description.Contains(searchString)));
                List<Product> searchBySubCategoryList = new List<Product>(products.Where(p =>
                    categories.Any(c => c.SubCategories.Any(sc => sc.SubCategoryID == p.SubCategoryID && sc.SubCategoryName.Contains(searchString))))).ToList();
                List<Product> searchByCategoryList = new List<Product>(products.Where(p =>
                    categories.Any(c => c.SubCategories.Any(sc => sc.SubCategoryID == p.SubCategoryID && c.CategoryName.Contains(searchString))))).ToList();
                products = searchByNameList.Union(searchByDescriptionList).Union(searchBySubCategoryList).Union(searchByCategoryList).ToList();
            }

            for (int i = 0; i < products.Count; i++)
            {
                images = new List<Image>(repository.Images.Where(index => index.ProductID == products[i].ProductID));
                productAndImages.Add(new ProductWithImages()
                {
                    product = products[i],
                    Images = images
                });
            }

            ProductsListViewModel productsListViewModel = new ProductsListViewModel { ProductAndImages = productAndImages.ToList(), Categories = categories};
            TempData["searchString"] = searchString;
            return View(productsListViewModel);
        }
        
        public ViewResult ViewSingleProduct(int productId, bool wasError)
        {
            if (wasError)
            {
                ModelState.AddModelError("", "Выберите цвет и фурнитуру");
            }
            Product selectedProduct = repository.Products.Where(h => h.HideFromUsers == false).FirstOrDefault(p => p.ProductID == productId);
            List<TypeColor> bindedColors = repository.TypeColors.Where(tci =>
                (repository.BoundColors.Where(i => i.ProductID == selectedProduct.ProductID).ToList()).Any(bci =>
                    bci.TypeColorID == tci.TypeColorID)).ToList();
            if (selectedProduct == null)
            {
                return View("List");
            }
            List<Product> relatedProducts = repository.ProductsWithImages
                .Where(sc => sc.SubCategoryID == selectedProduct.SubCategoryID
                && sc.ProductID != selectedProduct.ProductID).Take(5).ToList();
            List<Image> productImages = repository.Images.Where(p => p.ProductID == productId).ToList();
            selectedProduct.Images = productImages;
            var subCategory = 
                repository.SubCategories.First(sCId => sCId.SubCategoryID == selectedProduct.SubCategoryID);
            TempData["Category"] = repository.Categories.First(cId => cId.CategoryID == subCategory.CategoryID).CategoryName;
            TempData["SubCategory"] = subCategory.SubCategoryName;
            TempData["BindedColors"] = bindedColors;
            TempData["relatedProducts"] = relatedProducts;
            return View(selectedProduct);
        }
    }
}