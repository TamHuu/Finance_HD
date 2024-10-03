using System;
using System.Collections.Generic;

namespace Finance_HD.Models.HDEntity;

public partial class FiaBangKeNopTien
{
    public Guid Ma { get; set; }

    public Guid? MaChiNhanhNhan { get; set; }

    public Guid? MaChiNhanhNop { get; set; }

    public Guid? MaPhongBanNop { get; set; }

    public Guid? MaPhongBanNhan { get; set; }

    public string? SoPhieu { get; set; }

    public DateTime? NgayLap { get; set; }

    public DateTime? NgayNopTien { get; set; }

    public Guid? NguoiNopTien { get; set; }

    public string? TenNguoiNopTien { get; set; }

    public string? DiaChi { get; set; }

    public string? LyDo { get; set; }

    public Guid? MaTienTe { get; set; }

    public decimal? TyGia { get; set; }

    public decimal? SoTien { get; set; }

    public string? GhiChu { get; set; }

    public Guid? NguoiNhanTien { get; set; }

    public Guid? MaNoiDung { get; set; }

    public int? TrangThai { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? MaHinhThuc { get; set; }
}
