namespace Finance_HD.Helpers
{
    public static class UserHelper
    {
        public static string GetLoggedInUserGuid(HttpRequest request)
        {
            // Kiểm tra xem cookie "UserGuid" có tồn tại không
            if (request.Cookies.ContainsKey("UserName"))
            {
                // Trả về giá trị của cookie "UserGuid"
                return request.Cookies["UserName"];
            }
            // Nếu không có cookie, trả về chuỗi trống hoặc giá trị mặc định
            return string.Empty;
        }
    }
}
