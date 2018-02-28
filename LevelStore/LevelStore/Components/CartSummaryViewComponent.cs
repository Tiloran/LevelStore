using LevelStore.Models;
using LevelStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LevelStore.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart cart;
        private readonly IShareRepository shareRepository;

        public CartSummaryViewComponent(Cart cartService, IShareRepository sharesRepo)
        {
            cart = cartService;
            shareRepository = sharesRepo;
        }

        public IViewComponentResult Invoke()
        {
            CartWithSharesViewModel cartWithShares = new CartWithSharesViewModel
            {
                Cart = cart,
                Shares = shareRepository.Shares
            };      
            
            return View(cartWithShares);
        }
    }
}
