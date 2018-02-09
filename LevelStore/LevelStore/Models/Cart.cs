using System.Collections.Generic;
using System.Linq;

namespace LevelStore.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity, int furniture, int selectedColor)
        {
            CartLine line = lineCollection.FirstOrDefault(p => p.Product.ProductID == product.ProductID && p.Furniture == furniture && p.SelectedColor == selectedColor);

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity,
                    Furniture = furniture,
                    SelectedColor = selectedColor
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        }

        public virtual decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Quantity * e.Product.Price);
        }

        public virtual void Clear()
        {
            lineCollection.Clear();
        }

        public virtual IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }

    }


    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public int Furniture { get; set; }
        public int SelectedColor { get; set; }
        public int Quantity { get; set; }
    }
}
