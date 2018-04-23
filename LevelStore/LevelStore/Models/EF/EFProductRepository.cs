using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LevelStore.Models.EF
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;

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
        public IEnumerable<Promo> PromoCodes => context.PromoCodes;

        public void DeletePhoto(int productId, int photoId)
        {
            Product product = ProductsWithImages.FirstOrDefault(i => i.ProductID == productId);
            Image image = product?.Images.FirstOrDefault(i => i.ImageID == photoId);
            if (image != null)
            {
                if (System.IO.File.Exists("/images/" + image.Name))
                {
                    System.IO.File.Delete("wwwroot/images/" + image.Name);
                }
                product.Images.Remove(image);
                context.SaveChanges();
            }
        }

        public int UpdatePromo(Promo promo)
        {
            Promo updatePromo = context.PromoCodes.FirstOrDefault(i => i.PromoId == promo.PromoId);
            if (updatePromo != null)
            {
                context.Entry(updatePromo).State = EntityState.Detached;
                context.PromoCodes.Update(promo);
            }
            else
            {
                context.PromoCodes.Add(promo);
            }
            context.SaveChanges();

            return promo.PromoId;
        }

        public void DeletePromo(int promoId)
        {
            Promo deletePromo = context.PromoCodes.FirstOrDefault(i => i.PromoId == promoId);
            if (deletePromo != null)
            {
                context.PromoCodes.Remove(deletePromo);
                context.SaveChanges();
            }
        }

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
        public int? SaveProduct(Product product, List<int> colorsId = null)
        {
            if (colorsId == null)
            {
                colorsId = new List<int>();
            }
            if (colorsId.Count == 0)
            {
                if (context.TypeColors.FirstOrDefault() != null)
                {
                    colorsId.Add(context.TypeColors.First().TypeColorID);
                }
            }
            Product productToSave;
            if (product.ProductID == 0)
            {
                productToSave = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    SubCategoryID = product.SubCategoryID,
                    NewProduct = product.NewProduct,
                    Size = product.Size,
                    Description = product.Description,
                    HideFromUsers = product.HideFromUsers,
                    ViewsCounter = product.ViewsCounter,
                    AddToCartCounter = product.AddToCartCounter,
                    BuyingCounter = product.BuyingCounter
                };
                if (product.DateOfCreation == null)
                {
                    productToSave.DateOfCreation = DateTime.Now;
                }
                
                context.Products.Add(productToSave);
            }
            else
            {
                productToSave = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                if (productToSave != null)
                {
                    productToSave.Name = product.Name;
                    productToSave.Description = product.Description;
                    productToSave.Price = product.Price;
                    productToSave.SubCategoryID = product.SubCategoryID;
                    productToSave.NewProduct = product.NewProduct;
                    productToSave.Size = product.Size;
                    productToSave.HideFromUsers = product.HideFromUsers;
                    productToSave.ShareID = product.ShareID;
                    productToSave.AddToCartCounter = product.AddToCartCounter;
                    productToSave.BuyingCounter = product.BuyingCounter;
                    productToSave.ViewsCounter = product.ViewsCounter;
                    productToSave.DateOfCreation = productToSave.DateOfCreation == null ? DateTime.Now : product.DateOfCreation;
                }
            }
            context.SaveChanges();


            var save = productToSave;
            productToSave = context.Products.Where(i => i.ProductID == save.ProductID).Include(c => c.Color).First();
                if (productToSave.Color == null)
                {
                    productToSave.Color = new List<Color>();
                }
                List<int> productcolorsId = context.Colors.Where(i => i.ProductID == productToSave.ProductID).Select(id => id.TypeColorID).ToList();
                foreach (var colorid in colorsId)
                {
                    if (productcolorsId.Contains(colorid))
                    {
                        continue;
                    }
                    productToSave.Color.Add(new Color() { TypeColorID = colorid, ProductID = productToSave.ProductID });
                }
                foreach (var productcolorId in productcolorsId)
                {
                    if (colorsId.Contains(productcolorId))
                    {
                        continue;
                    }
                    productToSave.Color.Remove(productToSave.Color.FirstOrDefault(i => i.ColorID == productcolorId));
                }
                context.SaveChanges();
                return productToSave.ProductID;
        }

        public void AddImages(List<string> images, int? id)
        {
            Product product = context.Products.Where(p => p.ProductID == id).Include(i => i.Images).FirstOrDefault();
            TypeColor firstColor = context.TypeColors.FirstOrDefault();
            if (product != null)
            {
                if (product.Images == null)
                {
                    product.Images = new List<Image>();
                }
                bool setFirst = false;
                bool setSecond = false;
                if (!product.Images.Any())
                {
                    if (images.Count >= 2)
                    {
                        setFirst = true;
                        setSecond = true;
                    }
                    else if (images.Count == 1)
                    {
                        setFirst = true;
                    }
                }
                for (int i = 0; i < images.Count; i++)
                {
                    if (firstColor != null)
                    {
                        if (setFirst && i == 0)
                        {
                            product.Images.Add(new Image
                            {
                                Name = images[i],
                                FirstOnScreen = true,
                                TypeColorID = firstColor.TypeColorID
                            });
                        }
                        else if (setSecond && i == 1)
                        {
                            product.Images.Add(new Image
                            {
                                Name = images[i],
                                SecondOnScreen = true,
                                TypeColorID = firstColor.TypeColorID
                            });
                        }
                        else
                        {
                            product.Images.Add(new Image
                            {
                                Name = images[i],
                                TypeColorID = firstColor.TypeColorID
                            });
                        }

                    }
                    else
                    {
                        if (setFirst && i == 0)
                        {
                            product.Images.Add(new Image { Name = images[i], FirstOnScreen = true});
                        }
                        else if (setSecond && i == 1)
                        {
                            product.Images.Add(new Image { Name = images[i], SecondOnScreen = true});
                        }
                        else
                        {
                            product.Images.Add(new Image { Name = images[i] });
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        public List<TypeColor> GetColorThatBindedWithImages(List<Image> images)
        {
            List<TypeColor> bindedColors = context.TypeColors.Where(i1 => images.Any(i2 => i2.TypeColorID == i1.TypeColorID)).ToList();
            return bindedColors;
        }


        public void DeleteProduct(int? productId)
        {
            context.Colors.RemoveRange(context.Colors.Where(i => i.ProductID == productId).ToList());
            context.Images.RemoveRange(context.Images.Where(i => i.ProductID == productId).ToList());
            context.Products.Remove(context.Products.First(i => i.ProductID == productId));
            context.SaveChanges();
        }

        public void AddAnAddOnCountToTheCart(int productId)
        {
            Product product = Products.FirstOrDefault(i => i.ProductID == productId);
            if (product != null)
            {
                product.AddToCartCounter++;
                context.SaveChanges();
            }
        }

        public void AddViewCount(int productId)
        {
            Product product = Products.FirstOrDefault(i => i.ProductID == productId);
            if (product != null)
            {
                product.ViewsCounter++;
                context.SaveChanges();
            }
        }

        public void AddBuyCount(int productId)
        {
            Product product = Products.FirstOrDefault(i => i.ProductID == productId);
            if (product != null)
            {
                product.BuyingCounter++;
                context.SaveChanges();
            }
        }


        public List<Category> GetCategoriesWithSubCategories()
        {
            List<Category> categories = new List<Category>(context.Categories.Include(sc => sc.SubCategories));
            return categories;
        }
    }
}
