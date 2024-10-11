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
        [CustomAuthorize] // Áp dụng Action Filter
        public IActionResult Index()
        {
            // Kiểm tra nếu cookie có tồn tại
            if (Request.Cookies["FullName"] != null)
            {
                // Lấy giá trị từ cookie và gán vào ViewData
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
