using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Controllers
{
    public class DeliveryAndPaymentController : Controller
    {
        public ViewResult Guarantees() => View();

        public ViewResult Payment() => View();

        public ViewResult Delivery() => View();
    }
}