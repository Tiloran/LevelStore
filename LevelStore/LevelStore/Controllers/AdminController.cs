using System.Collections.Generic;
using System.IO;
using System.Linq;
using LevelStore.Models;
using LevelStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace LevelStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IHostingEnvironment _appEnvironment;

        public AdminController(IProductRepository repo, IHostingEnvironment appEnvironment)
        {
            _repository = repo;
            _appEnvironment = appEnvironment;
        }

        public ViewResult Create()
        {            
            TempData["Categories"] = _repository.GetCategoriesWithSubCategories().ToList();
            TempData["Colors"] = _repository.TypeColors.ToList();
            TempData["BoundColors"] = new List<Color>();
            return View("Edit", new Product());
        }

        public IActionResult Delete(int? productId)
        {
            if (productId != null)
            {
                _repository.DeleteProduct(productId);
            }
            return RedirectToAction("ListAdmin");
        }

        
        public IActionResult Edit(int productid)
        {
            Product product = _repository.Products.FirstOrDefault(i => i.ProductID == productid);
            TempData["Categories"] = _repository.GetCategoriesWithSubCategories().ToList();
            TempData["Colors"] = _repository.TypeColors.ToList();
            TempData["BoundColors"] = _repository.BoundColors.Where(i=> i.ProductID == productid).ToList();
            return View("Edit", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, List<int> colors)
        {
            if (ModelState.IsValid)
            {
                int? shareId = _repository.Products.FirstOrDefault(i => i.ProductID == product.ProductID)?.ShareID;
                if (shareId != null)
                {
                    product.ShareID = shareId;
                }
                int? id = _repository.SaveProduct(product, colors);
                if (id == null)
                {
                    return View();
                }
                return RedirectToActionPermanent("Edit", new {productid = id});
            }
            TempData["Categories"] = _repository.GetCategoriesWithSubCategories().ToList();
            TempData["Colors"] = _repository.TypeColors.ToList();
            if (product.ProductID != 0)
            {
                int? tempId = product.ProductID;
                TempData["BoundColors"] = _repository.BoundColors.Where(i => i.ProductID == tempId).ToList();
            }
            else
            {
                TempData["BoundColors"] = new List<Color>();
            }
            return View(product);
        }

        public ViewResult ListAdmin(int? categoryId, int? subCategoryId)
        {
            List<ProductWithImages> productAndImages = new List<ProductWithImages>();
            List<Product> products;
            if (subCategoryId != null)
            {
                products = new List<Product>(_repository.Products.Where(pScId => pScId.SubCategoryID == subCategoryId).OrderBy(pId => pId.ProductID));
            }
            else if (categoryId != null)
            {
                List<SubCategory> subCategories =
                    new List<SubCategory>(_repository.SubCategories.Where(i => i.CategoryID == categoryId).ToList());
                products = new List<Product>(_repository.Products
                    .Where(pScId => subCategories.Any(sCId => pScId.SubCategoryID == sCId.SubCategoryID))
                    .OrderBy(pId => pId.ProductID));
            }
            else
            {
                products = new List<Product>(_repository.Products.OrderBy(p => p.ProductID));
            }

            foreach (var product in products)
            {
                var images = new List<Image>(_repository.Images.Where(index =>
                    index.ProductID == product.ProductID));
                productAndImages.Add(new ProductWithImages()
                    {
                        Product = product,
                        Images = images
                    });
            }

            List<Category> categories = _repository.GetCategoriesWithSubCategories();
            ProductsListViewModel productsListViewModel = new ProductsListViewModel { ProductAndImages = productAndImages.ToList(), Categories = categories };
            return View(productsListViewModel);
        }

        public IActionResult AddColors()
        {
            TempData["ColorList"] = _repository.TypeColors.ToList();
            return View(new TypeColor());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddColors(TypeColor newTypeColor)
        {
            _repository.SaveTypeColor(newTypeColor);
            return RedirectToActionPermanent("AddColors");
        }

        public IActionResult BindPhotoAndColor(int?[] imageId, int?[] colorId, string[] alternative, int? valFirstOnScreen, int? valSecondOnScreen)
        {
            if (imageId.Length == colorId.Length)
            {
                for (int i = 0; i < imageId.Length; i++)
                {
                    if (imageId.Any())
                    {
                        Image currentImage = _repository.Images.FirstOrDefault(iid => iid.ImageID == imageId[i]);
                        if (currentImage != null)
                        {
                            currentImage.Alternative = alternative[i];
                            if (imageId[i] == valFirstOnScreen)
                            {
                                currentImage.FirstOnScreen = true;
                                currentImage.SecondOnScreen = false;
                            }
                            else if (imageId[i] == valSecondOnScreen)
                            {
                                currentImage.FirstOnScreen = false;
                                currentImage.SecondOnScreen = true;
                            }
                            else
                            {
                                currentImage.FirstOnScreen = false;
                                currentImage.SecondOnScreen = false;
                            }

                            TypeColor selectedColor =
                                _repository.TypeColors.FirstOrDefault(tcid => tcid.TypeColorID == colorId[i]);
                            if (selectedColor != null)
                            {
                                if (selectedColor.Images == null)
                                {
                                    selectedColor.Images = new List<Image>();
                                }

                                selectedColor.Images.Add(currentImage);
                                _repository.SaveTypeColor(selectedColor);
                            }
                        }
                    }
                }
            }

            int? id = TempData["id"] as int?;
            if (id != null)
            {
                return RedirectToActionPermanent("UploadFiles", new {productid = id});
            }
            else
            {
                return RedirectToActionPermanent("ListAdmin", "Admin");
            }
            
        }

        public IActionResult RemoveColor(int typeColorId)
        {
            _repository.DeleteTypeColor(typeColorId);
            List<TypeColor> typeColors = _repository.TypeColors.ToList();
            TempData["ColorList"] = typeColors;
            return View("AddColors",new TypeColor());
        }
        

        public IActionResult UploadFiles(int productId)
        {
            if (productId < 1)
            {
                return NotFound();
            }
            int id = productId;
            TempData["id"] = id;
            List<Color> bindedColors = _repository.BoundColors.Where(i => i.ProductID == id).ToList();
            List<TypeColor> ourTypeColors = _repository.TypeColors
                .Where(i1 => bindedColors.Any(i2 => i2.TypeColorID == i1.TypeColorID)).ToList();
            List<Image> imageList = _repository.Images.Where(i => i.ProductID == id).ToList();
            if (imageList.Count > 1)
            {
                bool noFirst = imageList.FirstOrDefault(f => f.FirstOnScreen && f.SecondOnScreen == false) == null;
                if (noFirst)
                {
                    imageList.First(f => f.FirstOnScreen == false && f.SecondOnScreen == false).FirstOnScreen = true;
                }
                bool noSecond = imageList.FirstOrDefault(s => s.FirstOnScreen == false && s.SecondOnScreen) == null;
                if (noSecond)
                {
                    imageList.First(s => s.FirstOnScreen == false && s.SecondOnScreen == false)
                        .SecondOnScreen = true;
                }
                bool bug = imageList.FirstOrDefault(s => s.FirstOnScreen && s.SecondOnScreen) != null;
                if (bug)
                {
                    foreach (var image in imageList)
                    {
                        if (image.FirstOnScreen && image.SecondOnScreen)
                        {
                            image.FirstOnScreen = false;
                            image.SecondOnScreen = false;
                        }
                    }
                }
            }
            List<TypeColor> boundedColors = _repository.GetColorThatBindedWithImages(imageList);
            TempData["Colors"] = ourTypeColors;
            TempData["ImageList"] = imageList;
            TempData["BindedColors"] = boundedColors;
            return View();
        }

        public IActionResult DeletePhoto(int productId, int photoId)
        {
            Product product = _repository.Products.FirstOrDefault(i => i.ProductID == productId);
            
            if (product != null)
            {
                _repository.DeletePhoto(productId, photoId);
                return RedirectToActionPermanent("UploadFiles", new { productid = productId });
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadFiles(IList<IFormFile> files)
        {
            List<string> imageNameList = new List<string>();
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .ToString()
                    .Trim('"');
                imageNameList.Add(filename);
                filename = _appEnvironment.WebRootPath + $@"\images\{filename}";
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            int? id = TempData["id"] as int?;

            if (id != null)
            {
                _repository.AddImages(imageNameList, id);
            }
            
            TempData["id"] = id;
            List<Image> imageList = _repository.Images.Where(i => i.ProductID == id).ToList();
            List<TypeColor> boundedColors = _repository.GetColorThatBindedWithImages(imageList);
            List<Color> bindedColors = _repository.BoundColors.Where(i => i.ProductID == id).ToList();
            List<TypeColor> ourTypeColors = _repository.TypeColors
                .Where(i1 => bindedColors.Any(i2 => i2.TypeColorID == i1.TypeColorID)).ToList();
            TempData["Colors"] = ourTypeColors;
            TempData["ImageList"] = imageList;
            TempData["BindedColors"] = boundedColors;
            return View("UploadFiles");
        }

        [HttpPost]
        public IActionResult UploadFilesAjax()
        {
            long size = 0;
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .ToString()
                    .Trim('"');
                filename = _appEnvironment.WebRootPath + $@"\images\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            string message = $"{files.Count} file(s) / {size} bytes uploaded successfully!";
            return Json(message);
        }

        public IActionResult ListPromo() => View(_repository.PromoCodes);
        
        public IActionResult DeletePromo(int? promoId)
        {
            if (promoId >= 1)
            {
                _repository.DeletePromo((int) promoId);
            }
            else
            {
                return NotFound();
            }

            return RedirectToActionPermanent("ListPromo");
        }

        public IActionResult EditPromo(int? promoId)
        {
            if (promoId == null || promoId == 0)
            {
                return View(new Promo());
            }
            else
            {
                Promo newPromo = _repository.PromoCodes.FirstOrDefault(i => i.PromoId == promoId);
                if (newPromo != null)
                {
                    return View(newPromo);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPromo(Promo promo)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdatePromo(promo);
                return RedirectToActionPermanent("ListPromo");
            }

            return View(promo);
        }
    }
}