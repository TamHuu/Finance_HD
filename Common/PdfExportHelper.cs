using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Finance_HD.Common
{
    public class PdfCreator
    {
        public void CreatePdf(string filePath)
        {
            // Tạo một tài liệu PDF
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf);

                    // Thêm tiêu đề
                    document.Add(new Paragraph("Tiêu đề tài liệu PDF"));

                    // Thêm nội dung
                    document.Add(new Paragraph("Đây là nội dung của PDF được tạo bằng iTextSharp."));

                    // Đóng tài liệu
                    document.Close();
                }
            }
        }
    }
}
