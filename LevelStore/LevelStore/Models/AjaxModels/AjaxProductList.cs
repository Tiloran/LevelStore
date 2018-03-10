using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelStore.Models.ViewModels;

namespace LevelStore.Models.AjaxModels
{
    public class AjaxProductList
    {
        public IEnumerable<ProductWithImages> ProductAndImages { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Share> Shares { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string SearchString { get; set; }
    }
}
