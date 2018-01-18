using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        IEnumerable<Image> Images { get; }

        void SaveProduct(Product product, List<string> Images);

        Product DeleteProduct(int productID);
    }
}
