namespace Finance_HD.Common
{
    public static class CovertExtensions
    {
        public static int ToInt(this string value)
        {
            // Kiểm tra nếu giá trị là null hoặc chuỗi rỗng
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0; // Trả về giá trị mặc định là 0
            }

            // Thử chuyển đổi giá trị sang int
            if (int.TryParse(value, out int result))
            {
                return result; // Trả về giá trị đã chuyển đổi
            }

            return 0; // Nếu chuyển đổi không thành công, trả về 0
        }
    }
}
