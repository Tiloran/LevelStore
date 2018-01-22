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
        IEnumerable<Color> BoundColors { get; }
        void SaveTypeColor(TypeColor typeColor);
        void DeleteTypeColor(int typeColorId);
        int? SaveProduct(Product product, List<int> colorsID);
        void AddImages(List<string> images, int? id);
        List<TypeColor> GetColorThatBindedWithImages(List<Image> images);
        Product DeleteProduct(int productID);
    }
}
