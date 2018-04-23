using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LevelStore.Models;
using LevelStore.Models.AjaxModels;

namespace LevelStore.Controllers
{
    public class SharesController : Controller
    {
        private readonly IShareRepository _repository;
        private readonly IProductRepository _productRepository;

        public SharesController(IShareRepository repo, IProductRepository productRepo)
        {
            _repository = repo;
            _productRepository = productRepo;
        }

        // GET: Shares
        public IActionResult Index()
        {
            return View(_repository.Shares.ToList());
        }

        
        // GET: Shares/Edit/5
        public IActionResult Edit(int? shareid)
        {
            if (shareid == null || shareid == 0)
            {
                TempData["Categories"] = _productRepository.GetCategoriesWithSubCategories();
                return View(new Share{DateOfStart = DateTime.Now, DateOfEnd = DateTime.Now.AddDays(7)});
            }

            var share = _repository.Shares.SingleOrDefault(m => m.ShareId == shareid);
            if (share == null)
            {
                return NotFound();
            }
            TempData["Categories"] = _productRepository.GetCategoriesWithSubCategories();
            return View(share);
        }

        // POST: Shares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Edit(int shareid, Share share, int[] products)
        {
            if (shareid != share.ShareId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                List<Product> productsWithShare = _productRepository.Products.Where(sId => sId.ShareID == shareid).ToList();
                foreach (var product in productsWithShare)
                {
                    product.ShareID = null;
                    _productRepository.SaveProduct(product);
                }

                productsWithShare = _productRepository.Products.Where(i => products.Any(id => id.Equals(i.ProductID))).ToList();
                share.Products = new List<Product>(productsWithShare);

                _repository.SaveShare(share);
                return RedirectToAction(nameof(Index));
            }
            return View(share);
        }

        // GET: Shares/Delete/5
        public IActionResult Delete(int? shareid)
        {
            if (shareid == null)
            {
                return NotFound();
            }

            var share = _repository.Shares
                .SingleOrDefault(m => m.ShareId == shareid);
            if (share == null)
            {
                return NotFound();
            }

            return View(share);
        }

        // POST: Shares/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int shareid)
        {
            _repository.Delete(shareid);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult SharesAjaxSearch([FromBody] AjaxSearch searchShaves)
        {
            if (searchShaves != null)
            {
                List<Product> products;
                if (searchShaves.SubCategoryId != null && searchShaves.SubCategoryId != 0)
                {
                    products = _productRepository.Products.Where(i => i.SubCategoryID == searchShaves.SubCategoryId)
                        .ToList();
                }
                else if (searchShaves.CategoryId != null && searchShaves.CategoryId != 0)
                {
                    List<Category> categories = _productRepository.GetCategoriesWithSubCategories();
                    products = _productRepository.Products.Where(p =>
                        categories.Any(c =>
                            c.CategoryID == searchShaves.CategoryId &&
                            c.SubCategories.Any(sc => sc.SubCategoryID == p.SubCategoryID))).ToList();
                }
                else
                {
                    products = _productRepository.Products.ToList();
                }

                if (!string.IsNullOrEmpty(searchShaves.SearchString) && !string.IsNullOrWhiteSpace(searchShaves.SearchString))
                {
                    products = products.Where(p => p.Name.Contains(searchShaves.SearchString) || p.Description.Contains(searchShaves.SearchString)).ToList();
                }
                foreach (var product in products)
                {
                    product.Description = null;
                    product.SubCategoryID = null;
                    product.Color = null;
                    product.HideFromUsers = false;
                    product.Images = null;
                    product.Price = 0;
                    product.NewProduct = false;
                    product.Size = null;
                }
                return Json(products);
            }
            return Json(new List<Product>());
        }
    }
}
