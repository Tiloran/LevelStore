using System.Collections.Generic;
using System.Linq;
using DNTBreadCrumb.Core;
using LevelStore.Models;
using LevelStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Controllers
{
    [BreadCrumb(Title = "Магазин", UseDefaultRouteUrl = true, Order = 0, GlyphIcon = "fa fa-angle-double-right")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IShareRepository _shareRepository;

        public ProductController(IProductRepository repo, IShareRepository shareRepo)
        {
            _repository = repo;
            _shareRepository = shareRepo;
        }

        [BreadCrumb(Order = 1)]
        public ViewResult List(int? categoryId, int? subCategoryId, string searchString)
        {
            List<ProductWithImages> productAndImages = new List<ProductWithImages>();
            List<Product> products;
            if (subCategoryId != null)
            {
                products = new List<Product>(_repository.Products.Where(pScId => pScId.SubCategoryID == subCategoryId).Where(h => h.HideFromUsers == false).OrderBy(pId => pId.ProductID));
            }
            else if (categoryId != null)
            {
                List<SubCategory> subCategories =
                    new List<SubCategory>(_repository.SubCategories.Where(i => i.CategoryID == categoryId).ToList());
                products = new List<Product>(_repository.Products
                    .Where(pScId => subCategories.Any(sCId => pScId.SubCategoryID == sCId.SubCategoryID))
                    .Where(h => h.HideFromUsers == false)
                    .OrderBy(pId => pId.ProductID));
            }
            else
            {
                products = new List<Product>(_repository.Products.Where(h => h.HideFromUsers == false).OrderBy(p => p.ProductID));
            }

            List<Category> categories = _repository.GetCategoriesWithSubCategories();

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

            foreach (var product in products)
            {
                var images = new List<Image>(_repository.Images.Where(index => index.ProductID == product.ProductID));
                productAndImages.Add(new ProductWithImages
                {
                    Product = product,
                    Images = images
                });
            }

            ProductsListViewModel productsListViewModel = new ProductsListViewModel { ProductAndImages = productAndImages.ToList(), Categories = categories};
            TempData["Shares"] = _shareRepository.Shares.Where(i =>
                productsListViewModel.ProductAndImages.Any(id => i.ShareId == id.Product.ShareID)).ToList();
            TempData["searchString"] = searchString;
            return View(productsListViewModel);
        }

        [BreadCrumb(Title = "Товар", Order = 2, GlyphIcon = "fa fa-angle-double-right")]
        public IActionResult ViewSingleProduct(int productId, bool wasError)
        {
            if (!_repository.Products.Select(i => i.ProductID).Contains(productId))
            {
                return NotFound();
            }
            if (wasError)
            {
                ModelState.AddModelError("", "Выберите цвет и фурнитуру");
            }
            Product selectedProduct = _repository.Products.Where(h => h.HideFromUsers == false).FirstOrDefault(p => p.ProductID == productId);
            if (selectedProduct == null)
            {
                return NotFound();
            }
            List<TypeColor> bindedColors = _repository.TypeColors.Where(tci =>
                (_repository.BoundColors.Where(i => i.ProductID == selectedProduct.ProductID).ToList()).Any(bci =>
                    bci.TypeColorID == tci.TypeColorID)).ToList();
            List<Product> relatedProducts = _repository.ProductsWithImages
                .Where(sc => sc.SubCategoryID == selectedProduct.SubCategoryID
                && sc.ProductID != selectedProduct.ProductID).Take(5).ToList();
            List<Image> productImages = _repository.Images.Where(p => p.ProductID == productId).ToList();
            selectedProduct.Images = productImages;
            var subCategory = 
                _repository.SubCategories.First(sCId => sCId.SubCategoryID == selectedProduct.SubCategoryID);
            TempData["Category"] = _repository.Categories.First(cId => cId.CategoryID == subCategory.CategoryID).CategoryName;
            TempData["SubCategory"] = subCategory.SubCategoryName;
            TempData["BindedColors"] = bindedColors;
            TempData["relatedProducts"] = relatedProducts;
            if (selectedProduct.ShareID != null)
            {
                TempData["Share"] = _shareRepository.Shares.FirstOrDefault(i => i.ShareId == selectedProduct.ShareID);
            }

            HttpContext.SetCurrentBreadCrumbTitle(selectedProduct.Name);

            _repository.AddViewCount(selectedProduct.ProductID);
            return View(selectedProduct);
        }
    }
}