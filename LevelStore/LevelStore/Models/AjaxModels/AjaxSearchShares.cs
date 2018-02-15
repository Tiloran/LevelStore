using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models.AjaxModels
{
    public class AjaxSearchShares
    {
        public string searchString { get; set; }
        public int? subCategoryId { get; set; }
        public int? categoryId { get; set; }

    }
}
