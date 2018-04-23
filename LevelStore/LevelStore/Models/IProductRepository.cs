using System.Collections.Generic;

namespace LevelStore.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        IEnumerable<Image> Images { get; }
        IEnumerable<TypeColor> TypeColors { get; }
        IEnumerable<Color> BoundColors { get; }
        IEnumerable<SubCategory> SubCategories { get; }
        IEnumerable<Category> Categories { get; }
        IEnumerable<Product> ProductsWithImages { get; }
        IEnumerable<Promo> PromoCodes { get; }
        void SaveTypeColor(TypeColor typeColor);
        List<Category> GetCategoriesWithSubCategories();
        void DeleteTypeColor(int typeColorId);
        int? SaveProduct(Product product, List<int> colorsId = null);
        void AddImages(List<string> images, int? id);
        List<TypeColor> GetColorThatBindedWithImages(List<Image> images);
        void DeleteProduct(int? productId);
        void AddAnAddOnCountToTheCart(int productId);
        void AddViewCount(int productId);
        void AddBuyCount(int productId);
        int UpdatePromo(Promo promo);
        void DeletePromo(int promoId);
        void DeletePhoto(int productId, int photoId);
    }
}
