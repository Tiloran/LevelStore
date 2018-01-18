using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models.ViewModels
{
    public class ProductWithImages
    {
        public Product product { get; set; }
        public List<Image> Images { get; set; }
    }
}
