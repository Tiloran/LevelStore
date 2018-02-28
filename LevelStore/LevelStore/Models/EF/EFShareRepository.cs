using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LevelStore.Models.EF
{
    public class EFShareRepository : IShareRepository
    {
        private readonly ApplicationDbContext _context;

        public EFShareRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }


        public IEnumerable<Share> Shares => _context.Shares.Include(p => p.Products);

        public int? SaveShare(Share share)
        {
            if (share != null)
            {
                if (share.ShareId == 0)
                {
                    _context.Shares.Add(share);
                }
                else
                {
                    _context.Shares.Update(share);
                    _context.Products.UpdateRange(share.Products);
                }
                _context.SaveChanges();
                return share.ShareId;
            }
            return null;
        }

        public void Delete(int shareId)
        {
            Share share = _context.Shares.FirstOrDefault(i => i.ShareId == shareId);
            if (share != null)
            {
                List<Product> products = _context.Products.Where(i => i.ShareID == share.ShareId).ToList();
                if (products.Count > 0)
                {
                    foreach (var product in products)
                    {
                        product.ShareID = null;
                    }
                    _context.SaveChanges();
                }
                _context.Shares.Remove(share);
                _context.SaveChanges();
            }
        }
    }
}
