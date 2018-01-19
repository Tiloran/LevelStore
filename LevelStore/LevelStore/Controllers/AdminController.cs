using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LevelStore.Infrastructure;
using LevelStore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace LevelStore.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository repository;
        private IHostingEnvironment _appEnvironment;

        public AdminController(IProductRepository repo, IHostingEnvironment appEnvironment)
        {
            repository = repo;
            _appEnvironment = appEnvironment;
        }

        public ViewResult Create()
        {
            TempData["OtherStuffForProduct"] = new OtherStuffForProduct();
            TempData["Colors"] = repository.TypeColors.ToList();
            TempData["BoundColors"] = new List<Color>();
            return View("Edit", new Product());
        }

        
        public IActionResult Edit(int productid)
        {
            Product product = repository.Products.FirstOrDefault(i => i.ProductID == productid);
            TempData["OtherStuffForProduct"] = new OtherStuffForProduct();
            TempData["Colors"] = repository.TypeColors.ToList();
            TempData["BoundColors"] = repository.BoundColors.Where(i=> i.ProductID == productid).ToList();
            return View("Edit", product);
        }

        [HttpPost]
        public IActionResult Edit(Product product, List<int> colors)
        {
            if (ModelState.IsValid)
            {
                int? id = repository.SaveProduct(product, colors);
                if (id == null)
                {
                    return View();
                }
                TempData["id"] = id;
                TempData["ImageList"] = repository.Images.ToList();
                List<Color> bindetColors = repository.BoundColors.Where(i => i.ProductID == id).ToList();
                List<TypeColor> typeColors = repository.TypeColors.ToList();
                List<TypeColor> ourTypeColors = new List<TypeColor>();

                foreach (var bindetColor in bindetColors)
                {
                    foreach (var typeColor in typeColors)
                    {
                        if (typeColor.TypeColorID == bindetColor.TypeColorID)
                        {
                            ourTypeColors.Add(typeColor);
                        }
                    }
                }
                TempData["Colors"] = ourTypeColors;
                //TempData["Colors"] = repository.TypeColors.ToList();
                return View("UploadFiles");
            }
            //Some error
            return View();
        }

        public IActionResult AddColors()
        {
            TempData["ColorList"] = repository.TypeColors.ToList();
            return View(new TypeColor());
        }

        [HttpPost]
        public IActionResult AddColors(TypeColor newTypeColor)
        {
            repository.SaveTypeColor(newTypeColor);
            TempData["ColorList"] = repository.TypeColors.ToList();
            return View(new TypeColor());
        }

        public IActionResult BindPhotoAndColor(int imageID, int ColorID)
        {
            TempData["ImageList"] = repository.Images.ToList();
            TempData["Colors"] = repository.TypeColors.ToList();
            return View("UploadFiles");
        }

        public IActionResult RemoveColor(int typeColorId)
        {
            repository.DeleteTypeColor(typeColorId);
            List<TypeColor> typeColors = repository.TypeColors.ToList();
            TempData["ColorList"] = typeColors;
            return View("AddColors",new TypeColor());
        }



        public ViewResult Test() => View();

        [HttpPost]
        public ViewResult Test(IList<Image> list)
        {
            return View();
        }

        public IActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFiles(IList<IFormFile> files)
        {
            long size = 0;
            List<string> imageList = new List<string>();
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .ToString()
                    .Trim('"');
                    imageList.Add(filename);
                filename = _appEnvironment.WebRootPath + $@"\images\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            int? id = TempData["id"] as int?;
            //ViewBag.Message = $"{files.Count} file(s) / {size} bytes uploaded successfully!";
            if (id != null)
            {
                repository.AddImages(imageList, id);
            }
            
            return RedirectToAction(actionName: "List", controllerName: "Product");
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
    }
}