using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index(string searchString)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}