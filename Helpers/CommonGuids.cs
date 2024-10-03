using Microsoft.IdentityModel.Tokens;

namespace Finance_HD.Helpers
{
    public class CommonGuids
    {
        public static readonly Guid defaultUID = Guid.Empty; // Có thể thay đổi giá trị nếu cần
    }

    public static class GuidExtensions // Đổi tên lớp để phản ánh rằng đây là lớp mở rộng
    {
        public static Guid GetGuid(this string? str)
        {
            if (string.IsNullOrEmpty(str)) // Kiểm tra xem chuỗi có null hoặc rỗng không
            {
                return CommonGuids.defaultUID; // Trả về defaultUID nếu chuỗi rỗng
            }

            try
            {
                return Guid.Parse(str); // Chuyển đổi chuỗi thành Guid
            }
            catch (FormatException) // Bắt lỗi cụ thể cho định dạng không hợp lệ
            {
                return CommonGuids.defaultUID; // Trả về defaultUID nếu chuỗi không thể chuyển đổi
            }
            catch (Exception) // Bắt tất cả các loại lỗi khác
            {
                return CommonGuids.defaultUID; // Trả về defaultUID cho mọi lỗi khác
            }
        }
    }
}
