using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToActionPermanent(controllerName: "Product", actionName: "List");
        }
    }
}