using System.Collections.Generic;

namespace LevelStore.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        IEnumerable<Image> Images { get; }
        IEnumerable<TypeColor> TypeColors { get; }
        IEnumerable<Color> BoundColors { get; }
        IEnumerable<AccessorieForBag> Accessories { get; }
        IEnumerable<SubCategory> SubCategories { get; }
        IEnumerable<Category> Categories { get; }
        void SaveTypeColor(TypeColor typeColor);
        List<Category> GetCategoriesWithSubCategories();
        void DeleteTypeColor(int typeColorId);
        int? SaveProduct(Product product, List<int> colorsID);
        void AddImages(List<string> images, int? id);
        List<TypeColor> GetColorThatBindedWithImages(List<Image> images);
        Product DeleteProduct(int productID);
    }
}
