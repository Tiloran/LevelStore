using System.Collections.Generic;

namespace LevelStore.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<ProductWithImages> ProductAndImages { get; set; }
        public string CurrentCategory { get; set; }
    }
}
