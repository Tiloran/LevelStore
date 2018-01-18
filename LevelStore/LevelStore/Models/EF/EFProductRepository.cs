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

        public void SaveProduct(Product product, List<string> Images)

        {
            if (product.ProductID == 0)
            {
                Product newProduct = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    Category = product.Category,
                    Images = new List<Image>()
                };
                foreach (var name in Images)
                {
                    newProduct.Images.Add(new Image
                    {
                        Name = name
                    });
                }
                context.Products.Add(newProduct);
            }
            else
            {
                Product dbEntry = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int ProductId)
        {
            return new Product();
        }
    }
}
