using System;
using System.Collections.Generic;

namespace Finance_HD.Models;

public partial class FiaThuTienHang
{
    public Guid Ma { get; set; }

    public string? SoHoaDon { get; set; }

    public Guid? MaChiNhanh { get; set; }

    public Guid? MaPhongBan { get; set; }

    public DateTime? NgayThu { get; set; }

    public Guid? MaNhanVien { get; set; }

    public Guid? MaKh { get; set; }

    public int? HinhThucThu { get; set; }

    public Guid? MaTienTe { get; set; }

    public decimal? TyGia { get; set; }

    public decimal? SoTien { get; set; }

    public bool? XacNhanThuTien { get; set; }

    public bool? XacNhanNo { get; set; }

    public string? GhiChu { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public decimal? TienMat { get; set; }

    public decimal? TienNganHang { get; set; }

    public decimal? TienNo { get; set; }
}
