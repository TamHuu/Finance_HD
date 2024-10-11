using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.NganHang
{
    public class VietinbankController : Controller
    {
        public IActionResult Index()
        {
            if (Request.Cookies["FullName"] != null)
            {
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
