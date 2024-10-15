using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Finance_HD.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthorize]
        public IActionResult Index()
        {
            if (Request.Cookies["FullName"] != null)
            {
                if (Request.Cookies["ExpireTime"] != null)
                {
                    DateTime expireTime;
                    if (DateTime.TryParse(Request.Cookies["ExpireTime"], out expireTime))
                    {
                        if (expireTime < DateTime.UtcNow) 
                        {
                            Response.Cookies.Delete("UserName");
                            Response.Cookies.Delete("FullName");
                            Response.Cookies.Delete("SDT");
                            Response.Cookies.Delete("ExpireTime");

                            return RedirectToAction("Login", "Account");
                        }
                    }
                }

                ViewData["FullName"] = Request.Cookies["FullName"];
            }
            else
            {
                ViewData["FullName"] = "Cookie không tồn tại";
            }

            return View();
        }
    }
}

