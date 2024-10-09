using Finance_HD.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ cho ứng dụng.
builder.Services.AddControllersWithViews();

// Thêm DbContext
var connectionString = builder.Configuration.GetConnectionString("HDFA");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình dịch vụ xác thực
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login"; // Trang đăng nhập
        options.LogoutPath = "/Account/Logout"; // Trang đăng xuất
        options.AccessDeniedPath = "/Account/AccessDenied"; // Trang truy cập bị từ chối
    });

var app = builder.Build();

// Cấu hình pipeline cho yêu cầu HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Thêm middleware xác thực
app.UseAuthorization();

// Route chính
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Chuyển hướng đến trang đăng nhập

app.Run();
