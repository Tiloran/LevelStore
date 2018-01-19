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
        IEnumerable<TypeColor> TypeColors { get; }
        void SaveTypeColor(TypeColor typeColor);
        void DeleteTypeColor(int typeColorId);
        int SaveProduct(Product product);
        void AddImages(List<string> images, int? id);
        Product DeleteProduct(int productID);
    }
}
