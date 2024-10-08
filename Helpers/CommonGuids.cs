using Microsoft.IdentityModel.Tokens;

namespace Finance_HD.Helpers
{
    public class CommonGuids
    {
        // Thay đổi giá trị của defaultUID
        public static Guid defaultUID = new Guid(); // Hoặc Guid.Parse("00000000-0000-0000-0000-000000000000");
    }

    public static class GuidExtensions
    {
        public static bool IsEmpty(this string? str)
        {
            return string.IsNullOrEmpty(str); // Sử dụng hàm có sẵn để kiểm tra chuỗi rỗng
        }

        public static Guid GetGuid(this string? str)
        {
            if (str.IsEmpty()) // Nếu chuỗi rỗng hoặc null
            {
                return Guid.Empty; // Trả về Guid.Empty nếu phòng ban không có
            }

            try
            {
                return Guid.Parse(str); // Cố gắng chuyển đổi chuỗi thành GUID
            }
            catch (FormatException) // Xử lý lỗi định dạng không hợp lệ
            {
                return Guid.Empty; // Trả về Guid.Empty khi lỗi xảy ra
            }
            catch (Exception) // Xử lý các lỗi khác
            {
                return Guid.Empty; // Trả về Guid.Empty cho bất kỳ lỗi nào
            }
        }

        public static Guid? GetGuidNull(this string? str)
        {
            try
            {
                return str.IsEmpty() ? null : Guid.Parse(str); // Nếu chuỗi rỗng trả về null
            }
            catch
            {
                return null; // Trả về null khi xảy ra lỗi
            }
        }
    }
}
