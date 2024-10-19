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
            // Check if the string is null or empty
            if (string.IsNullOrEmpty(str))
            {
                return Guid.Empty; // Return Guid.Empty if the input string is null or empty
            }

            try
            {
                // Attempt to parse the string into a GUID
                return Guid.Parse(str);
            }
            catch (FormatException)
            {
                // Return Guid.Empty if the format is invalid
                return Guid.Empty;
            }
            // Consider removing the catch for general exceptions unless you have a specific reason for it
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
