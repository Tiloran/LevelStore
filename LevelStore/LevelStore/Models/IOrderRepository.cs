using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelStore.Models.Enums;

namespace LevelStore.Models
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Orders { get; }
        void SaveOrder(Order order);
        void ChangeOrder(Order order);
        void ChangeStatus(OrderStatus status, int orderId);
    }
}
