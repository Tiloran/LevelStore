using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public int? SubCategoryID { get; set; }
        public string Size { get; set; }
        public List<Color> Color { get; set; }
        public bool NewProduct { get; set; }
        public bool HideFromUsers { get; set; }
        public List<Image> Images { get; set; }
        public int? ShareID { get; set; }
    }
}
