using Finance_HD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ cho ứng dụng.
builder.Services.AddControllersWithViews();

// Thêm DbContext
var connectionString = builder.Configuration.GetConnectionString("HDFA");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình xác thực bằng Cookie
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Cấu hình thêm cho Bearer Token (JWT)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer"; // Sử dụng Bearer token làm mặc định
    options.DefaultChallengeScheme = "Bearer"; // Sử dụng Bearer token làm mặc định
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Đọc từ cấu hình
        ValidAudience = builder.Configuration["Jwt:Audience"], // Đọc từ cấu hình
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Đọc từ cấu hình
    };
});

// Thêm chính sách xác thực
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BearerPolicy", policy =>
    {
        policy.AuthenticationSchemes.Add("Bearer");
        policy.RequireAuthenticatedUser();
    });
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

// Thêm middleware xác thực
app.UseAuthentication();
app.UseAuthorization();

// Route chính
app.MapControllers(); // Đảm bảo ánh xạ đến các controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
