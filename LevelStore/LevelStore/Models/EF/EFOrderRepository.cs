using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelStore.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LevelStore.Models.EF
{
    public class EFOrderRepository :  IOrderRepository
    {
        private ApplicationDbContext context;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Order> Orders => context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product).ThenInclude(c => c.Color);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }

        public void ChangeOrder(Order order)
        {
            if (order != null)
            {
                foreach (var line in order.Lines)
                {
                    line.Product = null;
                }
                context.Update(order);
                context.SaveChanges();
            }
        }

        public void ChangeStatus(OrderStatus status, int orderId)
        {
            Order order = context.Orders.FirstOrDefault(i => i.OrderID == orderId);
            if (order != null)
            {
                order.Status = (int) status;
                context.SaveChanges();
            }
        }
    }
}
