using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelStore.Models.EF;

namespace LevelStore.Models
{
    public static class AddColorsAndAcessories
    {
        public static void EnsurePopulated(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            if (!context.AccessoriesForBags.Any())
            {
                context.AccessoriesForBags.AddRange(
                    new AccessorieForBag { Name = "антик" },
                    new AccessorieForBag { Name = "никель" }
                );
            }
            if (!context.TypeColors.Any())
            {
                context.TypeColors.Add(new TypeColor {ColorType = "Черный"});
            }

            context.SaveChanges();
        }
    }
}
