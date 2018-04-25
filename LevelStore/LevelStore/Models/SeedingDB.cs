using System;
using System.Collections.Generic;
using System.Linq;
using LevelStore.Models.EF;
using Microsoft.EntityFrameworkCore;

namespace LevelStore.Models
{
    public static class SeedingDB
    {
        public static void EnsurePopulated(ApplicationDbContext context)
        {
            if (context.TypeColors.Any())
            {
                return;
            }

            if (!context.Users.Any())
            {
                context.Users.Add(new User {Email = "MyEmail@gmail.com", Password = "12345"});
            }
            if (!context.TypeColors.Any())
            {
                context.TypeColors.AddRange(
                    new TypeColor { ColorType = "Черный"},
                    new TypeColor { ColorType = "Белый" },
                    new TypeColor { ColorType = "Зеленый" },
                    new TypeColor { ColorType = "Малавья" }
                    );
            }
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(new Category
                {
                    CategoryName = "Кошельки",
                    SubCategories = new List<SubCategory>
                    {
                        new SubCategory { SubCategoryName = "CrazyHouse" },
                        new SubCategory { SubCategoryName = "Kaiser"},
                        new SubCategory { SubCategoryName = "Flatar"}
                    }
                },
                new Category
                {
                    CategoryName = "Мини-сумки",
                    SubCategories = new List<SubCategory>
                    {
                        new SubCategory { SubCategoryName = "CrazyHouse" },
                        new SubCategory { SubCategoryName = "Kaiser"},
                        new SubCategory { SubCategoryName = "Flatar"}
                    }
                },
                new Category
                {
                    CategoryName = "Сумки",
                    SubCategories = new List<SubCategory>
                    {
                        new SubCategory { SubCategoryName = "CrazyHouse" },
                        new SubCategory { SubCategoryName = "Kaiser"},
                        new SubCategory { SubCategoryName = "Flatar"}
                    }
                },
                new Category
                {
                    CategoryName = "Рюкзаки",
                    SubCategories = new List<SubCategory>
                    {
                        new SubCategory { SubCategoryName = "CrazyHouse" },
                        new SubCategory { SubCategoryName = "Kaiser"},
                        new SubCategory { SubCategoryName = "Flatar"}
                    }
                },
                new Category
                {
                    CategoryName = "Аксессуары",
                    SubCategories = new List<SubCategory>
                    {
                        new SubCategory { SubCategoryName = "CrazyHouse" },
                        new SubCategory { SubCategoryName = "Kaiser"},
                        new SubCategory { SubCategoryName = "Flatar"}
                    }
                },
                new Category
                {
                    CategoryName = "Limited Edition",
                    SubCategories = new List<SubCategory>
                    {
                        new SubCategory { SubCategoryName = "Limited Edition" }
                    }
                }
                );
            }
            context.SaveChanges();

            if (!context.Shares.Any())
            {
                context.Shares.Add(
                    new Share
                    {
                        DateOfStart = DateTime.Now,
                        DateOfEnd = DateTime.Now.AddYears(1),
                        KoefPrice = 50,
                        Enabled = true
                    });
            }
            context.SaveChanges();

            TypeColor firstColor = context.TypeColors.First();
            List<Category> categories = context.Categories.Include(sb => sb.SubCategories).ToList();
            Share share = context.Shares.First();

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Средний",
                        Description =
                            "Кошелек, который, станет для Вас незаменимым и надежным другом, так как благодаря своей компактности, всегда будет с Вами. Имеет одно отделение для мелочи на кнопке, одно большое отделение для купюр, два для кредитных карт и визиток. Закрывается на кнопку.",
                        NewProduct = true,
                        Price = 400,
                        DateOfCreation = DateTime.Now,
                        Size = "Размер: 11х10 см",
                        SubCategoryID = categories.First(c => c.CategoryName == "Кошельки").SubCategories
                            .First(sb => sb.SubCategoryName == "CrazyHouse").SubCategoryID
                        
                    },
                    new Product
                    {
                        Name = "Тревел-мини",
                        Description =
                            "Кошелек, который, станет для Вас незаменимым и надежным другом, так как благодаря своей компактности, всегда будет с Вами. Имеет одно отделение для мелочи на кнопке, одно большое отделение для купюр, два для кредитных карт и визиток. Закрывается на кнопку.",
                        NewProduct = true,
                        Price = 550,
                        DateOfCreation = DateTime.Now,
                        Size = "Размер: 20х10 см",
                        SubCategoryID = categories.First(c => c.CategoryName == "Кошельки").SubCategories
                            .First(sb => sb.SubCategoryName == "CrazyHouse").SubCategoryID

                    },
                    new Product
                    {
                        Name = "Левый",
                        Description =
                            "Кошелек, который поражает своей компактностью и оригинальностью дизайна. Имеет одно большое отделение для купюр, и три для кредитных карт и визиток. Закрывается на кнопку.",
                        NewProduct = true,
                        Price = 400,
                        DateOfCreation = DateTime.Now,
                        Size = "Размер: 10х9,5 см",
                        SubCategoryID = categories.First(c => c.CategoryName == "Кошельки").SubCategories
                            .First(sb => sb.SubCategoryName == "CrazyHouse").SubCategoryID

                    },
                    new Product
                    {
                        Name = "Капля",
                        Description =
                            "Компактная на вид, но весьма вместительная сумочка, которая позволит всегда носить с собой все необходимое. Имеет прочную тканевую подкладку, карман на молнии и одно большое отделение. Также у изделия присутствует длинная ручка на кобурной застежке. Закрывается на магниты.",
                        NewProduct = false,
                        Price = 1000,
                        DateOfCreation = DateTime.Now,
                        Size = "Размер: 23х17х8 см",
                        SubCategoryID = categories.First(c => c.CategoryName == "Мини-сумки").SubCategories.First(sb => sb.SubCategoryName == "Kaiser").SubCategoryID,
                        ShareID = share.ShareId
                    },
                    new Product
                    {
                        Name = "Крокодил",
                        Description =
                            "Хорошее путешествие начинается с правильно подобранной дорожной сумки. Ведь отправляясь отдыхать, хочется взять с собой все. А эта модель помещает в себя даже больше! Имеет одно большое отделение и два внутренних кармана.",
                        NewProduct = false,
                        Price = 4000,
                        DateOfCreation = DateTime.Now,
                        Size = "Размер: 32х53х40 см",
                        SubCategoryID = categories.First(c => c.CategoryName == "Сумки").SubCategories.First(sb => sb.SubCategoryName == "Flatar").SubCategoryID
                    },
                    new Product
                    {
                        Name = "Кукки",
                        Description =
                            "Компактный в размерах, но весьма вместительный и стильный рюкзак, который станет отличным вариантом как для неожиданных поездок, так и для прогулок по городу. Имеет прочную тканевую подкладку, внутренний карман на молнии, одно большое отделение и внешний карман на магнитах (13х18 см). Закрывается на молнию.",
                        NewProduct = false,
                        Price = 2000,
                        DateOfCreation = DateTime.Now,
                        Size = "Размер: 32х24х10 см",
                        SubCategoryID = categories.First(c => c.CategoryName == "Рюкзаки").SubCategories.First(sb => sb.SubCategoryName == "CrazyHouse").SubCategoryID
                    },
                    new Product
                    {
                        Name = "Ключница",
                        Description =
                            "Ключница - незаменимый, чрезвычайно полезный и удобный аксессуар, который также станет дополнением к Вашему повседневному образу. Вмешает в себя до 5-ти ключей.",
                        NewProduct = false,
                        Price = 150,
                        DateOfCreation = DateTime.Now,
                        SubCategoryID = categories.First(c => c.CategoryName == "Аксессуары").SubCategories.First(sb => sb.SubCategoryName == "Flatar").SubCategoryID
                    },
                    new Product
                    {
                        Name = "Пакет",
                        Description =
                            "Любители шопинга и больших покупок не обойдут стороной данную модель! Выполнена из ткани \"Художественный брезент\", имеет прочную тканевую подкладку, одно огромное отделение, внутренний кармашек на молнии и внешний карман на кнопке.",
                        NewProduct = false,
                        Price = 3000,
                        DateOfCreation = DateTime.Now,
                        Size = "Размер: 49х42х15 см",
                        SubCategoryID = categories.First(c => c.CategoryName == "Limited Edition").SubCategories.First(sb => sb.SubCategoryName == "Limited Edition").SubCategoryID
                    }
                );
            }
            context.SaveChanges();

            List<Product> products = context.Products.ToList();

            if (!context.Colors.Any())
            {
                context.Colors.AddRange(
                    new Color
                    {
                        ProductID = products.First(n => n.Name == "Средний").ProductID,
                        TypeColorID = firstColor.TypeColorID
                    },
                    new Color
                    {
                        ProductID = products.First(n => n.Name == "Тревел-мини").ProductID,
                        TypeColorID = firstColor.TypeColorID
                    },
                    new Color
                    {
                        ProductID = products.First(n => n.Name == "Левый").ProductID,
                        TypeColorID = firstColor.TypeColorID
                    },
                    new Color
                    {
                        ProductID = products.First(n => n.Name == "Капля").ProductID,
                        TypeColorID = firstColor.TypeColorID
                    },
                    new Color
                    {
                        ProductID = products.First(n => n.Name == "Крокодил").ProductID,
                        TypeColorID = firstColor.TypeColorID
                    },
                    new Color
                    {
                        ProductID = products.First(n => n.Name == "Кукки").ProductID,
                        TypeColorID = firstColor.TypeColorID
                    },
                    new Color
                    {
                        ProductID = products.First(n => n.Name == "Пакет").ProductID,
                        TypeColorID = firstColor.TypeColorID
                    });
            }
            context.SaveChanges();

            if (!context.Images.Any())
            {
                context.Images.AddRange(
                    new Image
                    {
                        Name = "Кошель(CrazyHouse)1.jpg",
                        FirstOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Средний").ProductID
                    },
                    new Image
                    {
                        Name = "Кошель(CrazyHouse)2.jpg",
                        SecondOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Средний").ProductID
                    },
                    new Image
                    {
                        Name = "Кошель(CrazyHouse)3.jpg",
                        FirstOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Тревел-мини").ProductID
                    },
                    new Image
                    {
                        Name = "Кошель(CrazyHouse)4.jpg",
                        SecondOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Тревел-мини").ProductID
                    },
                    new Image
                    {
                        Name = "Кошель(CrazyHouse)5.jpg",
                        FirstOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Левый").ProductID
                    },
                    new Image
                    {
                        Name = "Кошель(CrazyHouse)6.jpg",
                        SecondOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Левый").ProductID
                    },
                    new Image
                    {
                        Name = "Мини-Сумка(Kaiser1).jpg",
                        FirstOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Капля").ProductID
                    },
                    new Image
                    {
                        Name = "Мини-Сумка(Kaiser2).jpg",
                        SecondOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Капля").ProductID
                    },
                    new Image
                    {
                        Name = "Сумки(Flatar1).jpg",
                        FirstOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Крокодил").ProductID
                    },
                    new Image
                    {
                        Name = "Сумки(Flatar2).jpg",
                        SecondOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Крокодил").ProductID
                    },
                    new Image
                    {
                        Name = "Рюкзаки(CrazyHouse1).jpg",
                        FirstOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Кукки").ProductID
                    },
                    new Image
                    {
                        Name = "Рюкзаки(CrazyHouse2).jpg",
                        SecondOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Кукки").ProductID
                    },
                    new Image
                    {
                        Name = "Аксессуары(Flatar1).jpg",
                        FirstOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Ключница").ProductID
                    },
                    new Image
                    {
                        Name = "Аксессуары(Flatar2).jpg",
                        SecondOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Ключница").ProductID
                    },
                    new Image
                    {
                        Name = "LimitedEdition1.jpg",
                        FirstOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Пакет").ProductID
                    },
                    new Image
                    {
                        Name = "LimitedEdition2.jpg",
                        SecondOnScreen = true,
                        TypeColorID = firstColor.TypeColorID,
                        ProductID = products.First(n => n.Name == "Пакет").ProductID
                    });
            }
            context.SaveChanges();

            if (!context.Orders.Any())
            {
                for (int i = 1; i <= 10; i++)
                {
                    Random rand = new Random();
                    context.Orders.Add(
                        new Order
                        {
                            DateOfCreation = DateTime.Now.AddDays(i - 1),
                            FirstName = "Император",
                            LastName = "Теребитель " + i,
                            Address = "Белый дом",
                            City = "Вашингтон",
                            Comment = "Тестовая заявка",
                            Email = "Terebitel@gmail.com",
                            NovaPoshta = "№" + i,
                            Phone = "666",
                            Status = rand.Next(0,3)
                        });
                }
            }

            context.SaveChanges();
            List<Order> orders = context.Orders.ToList();

            if (!context.CartLines.Any())
            {
                foreach (var order in orders)
                {
                    Random rand = new Random();
                    for (int i = 0; i < rand.Next(1, 4); i++)
                    {
                        Product product = products.Skip(rand.Next(1, 6)).First();
                        int quantity = rand.Next(4, 10);
                        double? koef = null;
                        if (product.ShareID != null)
                        {
                            koef = 50;
                        }
                        context.CartLines.Add(
                            new CartLine
                            {
                                Furniture = 0,
                                SelectedColor = firstColor.TypeColorID,
                                Quantity = quantity,
                                OrderID = order.OrderID,
                                Product = product,
                                PriceAfterCheckout = quantity * product.Price,
                                KoefPriceAfterCheckout = koef

                            });
                    }

                }
            }
            context.SaveChanges();


        }
    }
}
