using System.Collections.Generic;

namespace LevelStore.Models.ViewModels
{
    public class CartWithSharesViewModel
    {
        public Cart Cart { get; set; }
        public IEnumerable<Share> Shares { get; set; }
    }
}
