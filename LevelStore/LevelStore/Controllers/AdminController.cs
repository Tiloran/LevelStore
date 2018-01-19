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
            TempData["CategoriesList"] = new CategoryList();
            return View("Edit", new Product());
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                int id = repository.SaveProduct(product);
                //TempData["message"] = $"{product.Name} has been saved";
                //ViewData["product"] = product;


                //string url = string.Format($"/UploadFiles?productname={product.Name} & productprice ={product.Price}");
                TempData["id"] = id;
                return RedirectToRoute(new {controller = "Admin",action ="UploadFiles" });
            }
            else
            {
                //Some error
                return View();
            }
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