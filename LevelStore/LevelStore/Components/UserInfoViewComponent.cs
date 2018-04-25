using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LevelStore.Components
{
    public class UserInfoViewComponent : ViewComponent
    {
        private IHttpContextAccessor _HttpContextAccessor;

        public UserInfoViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _HttpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            string userName = _HttpContextAccessor.HttpContext.User.Identity.Name;
            return View("UserInfo", userName);
        }
    }
}
