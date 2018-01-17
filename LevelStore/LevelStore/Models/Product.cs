using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public byte[] Picture { get; set; }
        public string Category { get; set; }
        public string Size { get; set; }
        public int Color { get; set; }
        public int? AccessoriesForBags { get; set; }
    }
}
