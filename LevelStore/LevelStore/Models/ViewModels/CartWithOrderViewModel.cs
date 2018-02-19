using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models.ViewModels
{
    public class CartWithOrderViewModel
    {
        public Cart cart { get; set; }
        public Order order { get; set; }
    }
}
