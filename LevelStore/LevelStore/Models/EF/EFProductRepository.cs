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
        public IEnumerable<TypeColor> TypeColors  => context.TypeColors;

        public void DeleteTypeColor(int typeColorId)
        {
            TypeColor deleteItem = context.TypeColors.FirstOrDefault(i => i.TypeColorID == typeColorId);
            if (deleteItem != null)
            {
                context.TypeColors.Remove(deleteItem);
                context.SaveChanges();
            }
        }

        public void SaveTypeColor(TypeColor typeColor)
        {
            context.TypeColors.Add(typeColor);
            context.SaveChanges();
        }
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
                    NewProduct = product.NewProduct,
                    Size = product.Size,
                    Description = product.Description
                    
                };
                
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
                    Product.NewProduct = product.NewProduct;
                    Product.Size = product.Size;
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
