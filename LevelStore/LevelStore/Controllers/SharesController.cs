using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LevelStore.Models;
using LevelStore.Models.AjaxModels;

namespace LevelStore.Controllers
{
    public class SharesController : Controller
    {
        private readonly IShareRepository repository;
        private readonly IProductRepository productRepository;

        public SharesController(IShareRepository repo, IProductRepository productRepo)
        {
            repository = repo;
            productRepository = productRepo;
        }

        // GET: Shares
        public IActionResult Index()
        {
            return View(repository.Shares.ToList());
        }

        
        // GET: Shares/Edit/5
        public IActionResult Edit(int? shareid)
        {
            if (shareid == null || shareid == 0)
            {
                return View(new Share());
            }

            var share = repository.Shares.SingleOrDefault(m => m.ShareId == shareid);
            if (share == null)
            {
                return NotFound();
            }
            TempData["Categories"] = productRepository.GetCategoriesWithSubCategories();
            return View(share);
        }

        // POST: Shares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Edit(int shareid, Share share, int[] Products)
        {
            if (shareid != share.ShareId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                List<Product> products = productRepository.Products.Where(sId => sId.ShareID == shareid).ToList();
                foreach (var product in products)
                {
                    product.ShareID = null;
                    productRepository.SaveProduct(product);
                }

                products = productRepository.Products.Where(i => Products.Any(id => id.Equals(i.ProductID))).ToList();
                share.Products = new List<Product>(products);

                repository.SaveShare(share);
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

            var share = repository.Shares
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
            repository.Delete(shareid);
            return RedirectToAction(nameof(Index));
        }

        private bool ShareExists(int shareid)
        {
            return repository.Shares.Any(e => e.ShareId == shareid);
        }

        [HttpPost]
        public IActionResult SharesAjaxSearch([FromBody] AjaxSearchShares searchShaves)
        {
            if (searchShaves != null)
            {
                List<Product> products;
                if (searchShaves.subCategoryId != null && searchShaves.subCategoryId != 0)
                {
                    products = productRepository.Products.Where(i => i.SubCategoryID == searchShaves.subCategoryId)
                        .ToList();
                }
                else if (searchShaves.categoryId != null && searchShaves.categoryId != 0)
                {
                    List<Category> categories = productRepository.GetCategoriesWithSubCategories();
                    products = productRepository.Products.Where(p =>
                        categories.Any(c =>
                            c.CategoryID == searchShaves.categoryId &&
                            c.SubCategories.Any(sc => sc.SubCategoryID == p.SubCategoryID))).ToList();
                }
                else
                {
                    products = productRepository.Products.ToList();
                }

                if (!string.IsNullOrEmpty(searchShaves.searchString) && !string.IsNullOrWhiteSpace(searchShaves.searchString))
                {
                    products = products.Where(p => p.Name.Contains(searchShaves.searchString) || p.Description.Contains(searchShaves.searchString)).ToList();
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
