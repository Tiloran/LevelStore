using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; }
        public string Size { get; set; }
        public List<Color> Color { get; set; }
        public bool NewProduct { get; set; }
        public int? AccessorieForBagID { get; set; }
        public List<Image> Images { get; set; }
    }
}
