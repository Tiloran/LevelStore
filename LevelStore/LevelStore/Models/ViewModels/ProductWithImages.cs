using System.Collections.Generic;

namespace LevelStore.Models.ViewModels
{
    public class ProductWithImages
    {
        public Product product { get; set; }
        public List<Image> Images { get; set; }
    }
}
