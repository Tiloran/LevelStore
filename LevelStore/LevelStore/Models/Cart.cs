﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using LevelStore.Models.EF;

namespace LevelStore.Models
{
    public class Cart
    {
        private readonly List<CartLine> lineCollection = new List<CartLine>();
        public int PromoCodeDiscount;
        public string PromoCodeName;

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

        public virtual void DeletePromoCode()
        {
            PromoCodeDiscount = 0;
            PromoCodeName = null;
        }

        public virtual void SetPromoCode(int prmCodeDisc, string prmCodeName)
        {
            PromoCodeDiscount = prmCodeDisc;
            PromoCodeName = prmCodeName;
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
        public decimal PriceAfterCheckout { get; set; }
        [Range(0, 100)]
        public double? KoefPriceAfterCheckout { get; set; }
        public bool? FakeShare { get; set; }
        public int Quantity { get; set; }
        public int? OrderID { get; set; }
    }
}
