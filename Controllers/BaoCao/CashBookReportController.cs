using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.BaoCao
{
    public class CashBookReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
