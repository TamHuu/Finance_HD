using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.BaoCao
{
    public class DanhSachBaoCaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
