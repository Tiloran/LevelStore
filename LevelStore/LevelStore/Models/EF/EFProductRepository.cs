using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
        public IEnumerable<Color> BoundColors => context.Colors;
        public IEnumerable<SubCategory> SubCategories => context.SubCategories;
        public IEnumerable<Category> Categories => context.Categories;
        public IEnumerable<Product> ProductsWithImages => context.Products.Include(i => i.Images);

        public void DeleteTypeColor(int typeColorId)
        {
            TypeColor deleteItem = context.TypeColors.FirstOrDefault(i => i.TypeColorID == typeColorId);
            if (deleteItem != null && context.TypeColors.Count() > 1)
            {
                List<Image> imagesWithDelatedColor =
                    context.Images.Where(i => i.TypeColorID == deleteItem.TypeColorID).ToList();
                List<Color> bindedColorsToProduct = context.Colors.Where(i => i.TypeColorID == deleteItem.TypeColorID).ToList();
                foreach (var image in imagesWithDelatedColor)
                {
                    image.TypeColorID = null;
                }
                foreach (var bindedColor in bindedColorsToProduct)
                {
                    context.Colors.Remove(bindedColor);
                }

                context.TypeColors.Remove(deleteItem);
                
                context.SaveChanges();
            }
        }

        public void SaveTypeColor(TypeColor typeColor)
        {
            if (typeColor.TypeColorID == 0)
            {
                context.TypeColors.Add(typeColor);
            }
            else
            {
                TypeColor editableColor = context.TypeColors.FirstOrDefault(tc => tc.TypeColorID == typeColor.TypeColorID);
                if (editableColor != null)
                {
                    editableColor.ColorType = typeColor.ColorType;
                    editableColor.Images = typeColor.Images;
                    editableColor.Color = typeColor.Color;
                }
            }
            
            context.SaveChanges();
        }
        public int? SaveProduct(Product product, List<int> colorsID)
        {
            if (colorsID.Count == 0)
            {
                if (context.TypeColors.FirstOrDefault() != null)
                {
                    colorsID.Add(context.TypeColors.FirstOrDefault().TypeColorID);
                }
            }
            Product Product = new Product();
            if (product.ProductID == 0)
            {
                Product = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    SubCategoryID = product.SubCategoryID,
                    NewProduct = product.NewProduct,
                    Size = product.Size,
                    Description = product.Description,
                    HideFromUsers = product.HideFromUsers
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
                    Product.SubCategoryID = product.SubCategoryID;
                    Product.NewProduct = product.NewProduct;
                    Product.Size = product.Size;
                    Product.HideFromUsers = product.HideFromUsers;
                }
            }
            context.SaveChanges();
            if (Product != null)
            {
                Product = context.Products.Where(i => i.ProductID == Product.ProductID).Include(c => c.Color).FirstOrDefault();
                if (Product.Color == null)
                {
                    Product.Color = new List<Color>();
                }
                List<int> ProductcolorsID = context.Colors.Where(i => i.ProductID == Product.ProductID).Select(id => id.TypeColorID).ToList();
                foreach (var colorid in colorsID)
                {
                    if (ProductcolorsID.Contains(colorid))
                    {
                        continue;
                    }
                    Product.Color.Add(new Color() { TypeColorID = colorid, ProductID = Product.ProductID });
                }
                foreach (var ProductcolorID in ProductcolorsID)
                {
                    if (colorsID.Contains(ProductcolorID))
                    {
                        continue;
                    }
                    Product.Color.Remove(Product.Color.FirstOrDefault(i => i.ColorID == ProductcolorID));
                }
                context.SaveChanges();
                return Product.ProductID;
            }
            return null;

        }

        public void AddImages(List<string> Images, int? id)
        {
            Product Product = context.Products.Where(p => p.ProductID == id).Include(i => i.Images).FirstOrDefault();
            TypeColor firstColor = context.TypeColors.FirstOrDefault();
            if (Product != null)
            {
                if (Product.Images == null)
                {
                    Product.Images = new List<Image>();
                }
                bool SetFirst = false;
                bool SetSecond = false;
                if (!Product.Images.Any())
                {
                    if (Images.Count >= 2)
                    {
                        SetFirst = true;
                        SetSecond = true;
                    }
                    else if (Images.Count == 1)
                    {
                        SetFirst = true;
                    }
                }
                for (int i = 0; i < Images.Count; i++)
                {
                    if (firstColor != null)
                    {
                        if (SetFirst && i == 0)
                        {
                            Product.Images.Add(new Image
                            {
                                Name = Images[i],
                                FirstOnScreen = true,
                                TypeColorID = firstColor.TypeColorID
                            });
                        }
                        else if (SetSecond && i == 1)
                        {
                            Product.Images.Add(new Image
                            {
                                Name = Images[i],
                                SecondOnScreen = true,
                                TypeColorID = firstColor.TypeColorID
                            });
                        }
                        else
                        {
                            Product.Images.Add(new Image
                            {
                                Name = Images[i],
                                TypeColorID = firstColor.TypeColorID
                            });
                        }

                    }
                    else
                    {
                        if (SetFirst && i == 0)
                        {
                            Product.Images.Add(new Image { Name = Images[i], FirstOnScreen = true});
                        }
                        else if (SetSecond && i == 1)
                        {
                            Product.Images.Add(new Image { Name = Images[i], SecondOnScreen = true});
                        }
                        else
                        {
                            Product.Images.Add(new Image { Name = Images[i] });
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        public List<TypeColor> GetColorThatBindedWithImages(List<Image> images)
        {
            //List<TypeColor> bindedColors = context.TypeColors
            //     .Include(c => c.Images.Where(i1 => images.Any(i2 => i2.TypeColorID == i1.TypeColorID))).ToList();
            List<TypeColor> bindedColors = context.TypeColors.Where(i1 => images.Any(i2 => i2.TypeColorID == i1.TypeColorID)).ToList();
            return bindedColors;
        }


        public void DeleteProduct(int? ProductId)
        {
            context.Colors.RemoveRange(context.Colors.Where(i => i.ProductID == ProductId).ToList());
            context.Images.RemoveRange(context.Images.Where(i => i.ProductID == ProductId).ToList());
            context.Products.Remove(context.Products.First(i => i.ProductID == ProductId));
            context.SaveChanges();
        }


        public List<Category> GetCategoriesWithSubCategories()
        {
            List<Category> categories = new List<Category>(context.Categories.Include(sc => sc.SubCategories));
            return categories;
        }
    }
}
