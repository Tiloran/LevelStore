using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}