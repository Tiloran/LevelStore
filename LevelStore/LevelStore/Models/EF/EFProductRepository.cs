using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models.EF
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;

        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Product> Products => context.Products;
        public IEnumerable<Image> Images => context.Images;

        public int SaveProduct(Product product)
        {
            Product Product = new Product();
            if (product.ProductID == 0)
            {
                Product = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    Category = product.Category,
                    //Images = product.Images.ToList()
                };
                //foreach (var name in Images)
                //{
                //    newProduct.Images.Add(new Image
                //    {
                //        Name = name
                //    });
                //}
                context.Products.Add(Product);
            }
            else
            {
                Product = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                if (Product != null)
                {
                    Product.Name = product.Name;
                    Product.Description = product.Description;
                    Product.Price = product.Price;
                    Product.Category = product.Category;
                }
            }
            context.SaveChanges();
            return Product.ProductID;
        }

        public void AddImages(List<string> Images, int? id)
        {
            Product Product = context.Products.FirstOrDefault(p => p.ProductID == id);
            if (Product != null)
            {
                if (Product.Images == null)
                {
                    Product.Images = new List<Image>();
                }
                foreach (var image in Images)
                {
                    Product.Images.Add(new Image { Name = image });
                }
                context.SaveChanges();
            }
        }

        public Product DeleteProduct(int ProductId)
        {
            return new Product();
        }
    }
}
