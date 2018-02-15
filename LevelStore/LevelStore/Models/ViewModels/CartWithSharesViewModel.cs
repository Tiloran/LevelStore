using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models.ViewModels
{
    public class CartWithSharesViewModel
    {
        public Cart cart { get; set; }
        public IEnumerable<Share> shares { get; set; }
    }
}
