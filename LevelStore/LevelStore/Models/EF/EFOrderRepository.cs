using System;
using System.Collections.Generic;
using System.Linq;
using LevelStore.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LevelStore.Models.EF
{
    public class EFOrderRepository :  IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Order> Orders => _context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product).ThenInclude(c => c.Color);

        public void SaveOrder(Order order)
        {            
            _context.CartLines.UpdateRange(order.Lines);
            _context.SaveChanges();
            if (order.OrderID == 0)
            {
                if (order.DateOfCreation == null)
                {
                    order.DateOfCreation = DateTime.Now;
                }
                _context.Orders.Add(order);
            }
            _context.SaveChanges();
        }

        public void ChangeOrder(Order order)
        {
            if (order != null)
            {
                foreach (var line in order.Lines)
                {
                    line.Product = null;
                }
                _context.Update(order);
                _context.SaveChanges();
            }
        }

        public void ChangeStatus(OrderStatus status, int orderId)
        {
            Order order = _context.Orders.FirstOrDefault(i => i.OrderID == orderId);
            if (order != null)
            {
                order.Status = (int) status;
                _context.SaveChanges();
            }
        }
    }
}
