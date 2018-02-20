using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LevelStore.Models.EF
{
    public class EFShareRepository : IShareRepository
    {
        private readonly ApplicationDbContext context;

        public EFShareRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }


        public IEnumerable<Share> Shares => context.Shares.Include(p => p.Products);

        public int? SaveShare(Share share)
        {
            if (share != null)
            {
                if (share.ShareId == 0)
                {
                    context.Shares.Add(share);
                }
                else
                {
                    context.Shares.Update(share);
                    context.Products.UpdateRange(share.Products);
                }
                context.SaveChanges();
                return share.ShareId;
            }
            return null;
        }

        public void Delete(int shareId)
        {
            Share share = context.Shares.FirstOrDefault(i => i.ShareId == shareId);
            if (share != null)
            {
                List<Product> products = context.Products.Where(i => i.ShareID == share.ShareId).ToList();
                if (products.Count > 0)
                {
                    foreach (var product in products)
                    {
                        product.ShareID = null;
                    }
                    context.SaveChanges();
                }
                context.Shares.Remove(share);
                context.SaveChanges();
            }
        }
    }
}
