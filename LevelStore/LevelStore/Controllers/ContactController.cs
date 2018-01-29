using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Controllers
{
    public class ContactController : Controller
    {
        public ViewResult Bayer() => View();

        public ViewResult DeliveryDepartment() => View();

        public ViewResult OurShowRoom() => View();

        public ViewResult Partnership() => View();

        public ViewResult Vacancy() => View();
    }
}