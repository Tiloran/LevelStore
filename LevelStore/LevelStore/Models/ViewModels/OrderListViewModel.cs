using System.Collections.Generic;

namespace LevelStore.Models.ViewModels
{
    public class OrderListViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
