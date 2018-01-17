using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Product { get; set; }
        public string CurrentCategory { get; set; }
    }
}
