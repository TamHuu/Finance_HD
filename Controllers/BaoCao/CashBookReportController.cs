using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.BaoCao
{
    public class CashBookReportController : Controller
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
