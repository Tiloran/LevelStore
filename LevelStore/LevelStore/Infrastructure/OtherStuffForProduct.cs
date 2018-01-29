using System.Collections.Generic;

namespace LevelStore.Infrastructure
{
    public class OtherStuffForProduct
    {
        public List<string> categories = new List<string>()
        {
            "purse-CrazyHouse",
            "purse-Kaiser",
            "purse-Flatar",
            "mini-bag-CrazyHouse",
            "mini-bag-Kaiser",
            "mini-bag-Flatar",
            "bag-CrazyHouse",
            "bag-Kaiser",
            "bag-Flatar",
            "backpacks-CrazyHouse",
            "backpacks-Kaiser",
            "backpacks-Flatar",
            "Accessories-CrazyHouse",
            "Accessories-Kaiser",
            "Accessories-Flatar",
            "LimitedEdition"
        };

        public Dictionary<int, string> Colors = new Dictionary<int, string>()
        {
            {1, "черный"},
            {2, "синий"},
            {3, "зеленый"},
            {4, "хаки" }
        };

        public Dictionary<int , string> AccessoriesForBags = new Dictionary<int, string>()
        {
            {1, "антик"},
            {2, "никель"}
        };



    }
}
