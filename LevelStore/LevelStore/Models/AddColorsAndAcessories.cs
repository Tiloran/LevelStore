using System.Collections.Generic;
using System.Linq;
using LevelStore.Models.EF;

namespace LevelStore.Models
{
    public static class AddColorsAndAcessories
    {
        public static void EnsurePopulated(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            if (!context.TypeColors.Any())
            {
                context.TypeColors.Add(new TypeColor {ColorType = "Черный"});
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
                    CategoryName = "Cумки",
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
        }
    }
}
