using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Finance_HD.Controllers.Login
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public LoginController(ILogger<LoginController> logger, ApplicationDbContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login([FromBody] SysUser model)
        {
            // Kiểm tra tính hợp lệ của mật khẩu
            if (!IsValidPassword(model.Password))
            {
                return Json(new { success = false, message = "Mật khẩu không hợp lệ." });
            }

            var user = _dbContext.SysUser.FirstOrDefault(x => x.Username == model.Username);

            // So sánh mật khẩu (giả định rằng mật khẩu được lưu dưới dạng plaintext, hãy xem xét sử dụng hash)
            if (user != null && user.Password == model.Password)
            {
                // Lấy khóa từ cấu hình
                var key = _configuration["Jwt:Key"];
                var keyBytes = Encoding.UTF8.GetBytes(key);

                // Tạo token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, user.Username),
                        // Bạn có thể thêm claim khác nếu cần
                    }),
                    Expires = DateTime.UtcNow.AddHours(1), // Thời gian sống của token
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Json(new { success = true, token = tokenString, message = "Bạn đã đăng nhập thành công" });
            }

            return Json(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không đúng." });
        }

        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8) // Kiểm tra độ dài
                return false;

            if (!password.Any(char.IsUpper)) // Kiểm tra có chữ hoa không
                return false;

            if (!password.Any(char.IsLower)) // Kiểm tra có chữ thường không
                return false;

            if (!password.Any(char.IsDigit)) // Kiểm tra có chữ số không
                return false;

            if (!password.Any(ch => "!@#$%^&*()_+[]{}|;':\",.<>?/`~".Contains(ch))) // Kiểm tra ký tự đặc biệt
                return false;

            // Có thể thêm các điều kiện khác nếu cần

            return true; // Mật khẩu hợp lệ
        }



    }
}
