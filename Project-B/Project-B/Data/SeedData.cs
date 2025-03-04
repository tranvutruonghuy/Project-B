using Microsoft.EntityFrameworkCore;
using Project_B.Models;

namespace Project_B.Data
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                CategoryModel thuoc = new CategoryModel { Id = 1, Name = "Thuốc", Description = "Thuốc kháng sinh", ParentID = -1 };
                CategoryModel duocMyPham = new CategoryModel { Id = 2, Name = "Dược mỹ phẩm", Description = "Dược và mỹ phẩm", ParentID = -1 };
                _context.Products.AddRange(
                    new ProductModel
                    {
                        Id = 1,
                        Name = "Kem Nizoral Jassen 20mg-gs",
                        CategoryId = 5,
                        Description = "<h2>C&ocirc;ng dụng của Kem Nizoral</h2>\r\n \r\n <h3>Chỉ định</h3>\r\n \r\n <p><a href=\"https://nhathuoclongchau.com.vn/thuoc/nizoral-janssen-cream-15-g.html\">Kem Nizoral</a>&nbsp;điều trị c&aacute;c nhiễm nấm ngo&agrave;i da: Nhiễm nấm ở th&acirc;n (l&aacute;c, hắc l&agrave;o), nhiễm nấm ở bẹn, nhiễm nấm ở b&agrave;n tay v&agrave; b&agrave;n ch&acirc;n do Trichophyton rubrum, Trichophyton mentagrophytes, Microsporum canis v&agrave; Epidermophyton floccosum, điều trị&nbsp;<a href=\"https://nhathuoclongchau.com.vn/benh/nhiem-candida-197.html\">nhiễm nấm Candida</a>&nbsp;ở da v&agrave; điều trị bệnh lang ben.</p>\r\n \r\n <p>Kem Nizoral c&ograve;n được chỉ định trong điều trị&nbsp;<a href=\"https://nhathuoclongchau.com.vn/benh/viem-da-tiet-ba-1364.html\">vi&ecirc;m da tiết b&atilde;</a>&nbsp;- một bệnh l&yacute; da li&ecirc;n quan đến sự hiện diện của nấm Malassezia furfur.</p>\r\n \r\n <h3>Dược lực học</h3>\r\n \r\n <p><a href=\"https://nhathuoclongchau.com.vn/thuoc/ketoconazole-200mg-3355.html\">Ketoconazole</a>, một dẫn xuất imidazole dioxolant tổng hợp, c&oacute; hoạt t&iacute;nh kh&aacute;ng nấm mạnh đối với c&aacute;c vi nấm ngo&agrave;i da như Trichophyton sp., Epidermophyton floccosum v&agrave; Microsporum sp. v&agrave; đối với c&aacute;c nấm men, bao gồm Malassezia spp. v&agrave; Candida spp. Đặc biệt hiệu quả tr&ecirc;n Malassezia spp. rất nổi bật.</p>\r\n \r\n <p>Ketoconazole ức chế sinh tổng hợp ergosterol ở nấm v&agrave; l&agrave;m thay đổi cấu tr&uacute;c c&aacute;c th&agrave;nh phần lipid kh&aacute;c của m&agrave;ng.</p>\r\n \r\n <p>Ketoconazole thường t&aacute;c dụng rất nhanh tr&ecirc;n triệu chứng ngứa, l&agrave; triệu chứng thường thấy ở c&aacute;c nhiễm nấm ngo&agrave;i da v&agrave; nấm men cũng như trong những bệnh da c&oacute; li&ecirc;n quan đến sự hiện diện của chủng nấm Malassezia spp..</p>\r\n \r\n <p>Giảm triệu chứng được ghi nhận trước khi thấy c&aacute;c dấu hiệu l&agrave;nh bệnh đầu ti&ecirc;n.</p>\r\n \r\n <h3>Dược động học</h3>\r\n \r\n <p>Đối với người lớn, sau khi thoa tại chỗ Nizoral 2% tr&ecirc;n da kh&ocirc;ng tạo ra được một nồng độ c&oacute; thể ph&aacute;t hiện ở m&aacute;u. Trong một nghi&ecirc;n cứu ở trẻ nhũ nhi bị vi&ecirc;m da tiết b&atilde; (n=19), cho d&ugrave;ng khoảng 40g kem Nizoral mỗi ng&agrave;y tr&ecirc;n 40% diện t&iacute;ch bề mặt da, ph&aacute;t hiện nồng độ huyết tương ketoconazole ở 5 trẻ, ở mức 32-133 ng/ml.</p>\r\n \r\n <p><em>Dữ liệu an to&agrave;n tiền l&acirc;m s&agrave;ng</em></p>\r\n \r\n <p>Kh&ocirc;ng c&oacute; ph&aacute;t hiện rủi ro đặc biệt n&agrave;o cho con người dựa tr&ecirc;n c&aacute;c kết quả nghi&ecirc;n cứu đ&aacute;nh gi&aacute; k&iacute;ch ứng da hoặc mắt, vi&ecirc;m da v&agrave; độc t&iacute;nh tr&ecirc;n da khi sử dụng lặp lại.</p>\r\n \r\n <p>Nghi&ecirc;n cứu cấp t&iacute;nh k&iacute;ch ứng da v&agrave; mắt với c&ocirc;ng thức kem ketoconazole tr&ecirc;n thỏ cho thấy kh&ocirc;ng c&oacute; k&iacute;ch ứng da hoặc mắt. Kết quả từ một nghi&ecirc;n cứu về nhạy cảm da ở chuột lang cho thấy kh&ocirc;ng c&oacute; tiềm năng g&acirc;y dị ứng hoặc mẫn cảm. Trong năm nghi&ecirc;n cứu về da lặp lại tr&ecirc;n thỏ, ketoconazole được d&ugrave;ng cho cả da bị trầy xước v&agrave; kh&ocirc;ng bị trầy xước ở liều tối đa l&agrave; 40 mg/kg. Trong một nghi&ecirc;n cứu một số k&iacute;ch ứng da nhẹ đ&atilde; được ghi nhận trong cả hai nh&oacute;m ketoconazole v&agrave; giả dược, tuy nhi&ecirc;n, trong c&aacute;c nghi&ecirc;n cứu c&ograve;n lại kh&ocirc;ng c&oacute; trường hợp phản ứng da hay hiệu ứng độc hai n&agrave;o được ghi nhận. Dữ liệu từ c&aacute;c nghi&ecirc;n cứu dược động học, một số c&ocirc;ng thức d&ugrave;ng ngo&agrave;i của ketoconazole trong điều kiện thử nghiệm ph&oacute;ng đại ở động vật th&iacute; nghiệm, cho thấy kh&ocirc;ng ph&aacute;t hiện nồng độ ketoconazole trong huyết tương.</p>\r\n ",
                        ShortDescription = "Điều trị nhiễm giun",
                        InStock = 1500,
                        Price = 71000,
                        Unit = "Hộp",
                        Image = "Product_54_2025-03-04_17-11-37_377_kem1.webp"
                    }                   
                );
                _context.SaveChanges();
            }
        }
    }
}
