using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.ChungTu
{
    public class ExpenseRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
