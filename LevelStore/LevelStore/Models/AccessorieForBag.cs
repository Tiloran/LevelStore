using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LevelStore.Models
{
    public class AccessorieForBag
    {
        public int AccessorieForBagID { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
