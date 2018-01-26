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

        public ViewResult ListAdmin(string category)
        {
            List<ProductWithImages> productAndImages = new List<ProductWithImages>();
            List<Product> products = new List<Product>();
            List<Image> images = new List<Image>();
            products = new List<Product>(repository.Products.Where(p => category == null || p.Category.Contains(category)).OrderBy(p => p.ProductID));
            images = new List<Image>(repository.Images);

            for (int i = 0; i < products.Count; i++)
            {
                images = new List<Image> (repository.Images.Where(index => index.ProductID == products[i].ProductID));
                productAndImages.Add(new ProductWithImages()
                {
                    product = products[i],
                    Images = images
                });
            }

            ProductsListViewModel productsListViewModel = new ProductsListViewModel {ProductAndImages = productAndImages.ToList()};
            return View(productsListViewModel);
        }

        public ViewResult List(string category)
        {
            List<ProductWithImages> productAndImages = new List<ProductWithImages>();
            List<Product> products = new List<Product>();
            List<Image> images = new List<Image>();
            products = new List<Product>(repository.Products.Where(p => category == null || p.Category.Contains(category)).OrderBy(p => p.ProductID));
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

            ProductsListViewModel productsListViewModel = new ProductsListViewModel { ProductAndImages = productAndImages.ToList() };
            return View(productsListViewModel);
        }

        

        [HttpPost]
        public ViewResult ViewSingleProduct(int productId)
        {
            Product selectedProduct = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            List<Image> productImages = repository.Images.Where(p => p.ProductID == productId).ToList();
            selectedProduct.Images = productImages;
            return View(selectedProduct);
        }
    }
}