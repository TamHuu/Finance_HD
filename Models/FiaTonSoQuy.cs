using System;
using System.Collections.Generic;

namespace Finance_HD.Models.HDEntity;

public partial class FiaTonSoQuy
{
    public Guid Ma { get; set; }

    public int? Nam { get; set; }

    public int? Thang { get; set; }

    public Guid? MaChiNhanh { get; set; }

    public Guid? MaPhongBan { get; set; }

    public Guid? MaTienTe { get; set; }

    public int? HinhThucGiaoDich { get; set; }

    public Guid? MaTaiKhoanGiaoDich { get; set; }

    public decimal? SoDuDauKy { get; set; }

    public decimal? ThuTrongKy { get; set; }

    public decimal? ChiTrongKy { get; set; }

    public decimal? ChiTam { get; set; }

    public decimal? TonCuoiKy { get; set; }

    public Guid? MaNganHang { get; set; }

    public string? SoTaiKhoanKhachHang { get; set; }
}
