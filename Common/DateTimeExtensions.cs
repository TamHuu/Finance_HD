
namespace Finance_HD.Common
{
    public static class DateTimeExtensions
    {
        public static DateTime? ToDateTime2(this string dateString, DateTime defaultDate)
        {
            // Kiểm tra xem chuỗi có giá trị hay không
            if (string.IsNullOrEmpty(dateString))
            {
                return defaultDate;
            }

            // Thử chuyển đổi chuỗi thành DateTime
            if (DateTime.TryParse(dateString, out DateTime result))
            {
                return result;
            }

            // Nếu không chuyển đổi được, trả về null
            return null;
        }
    }

}
