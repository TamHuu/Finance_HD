using System;
using System.Collections.Generic;

namespace Finance_HD.Models;

public partial class FiaDeNghiChi
{
    public Guid Ma { get; set; }

    public Guid? MaChiNhanhDeNghi { get; set; }

    public Guid? MaChiNhanhChi { get; set; }

    public Guid? MaPhongBanDeNghi { get; set; }

    public Guid? MaPhongBanChi { get; set; }

    public string? SoPhieu { get; set; }

    public DateTime? NgayLap { get; set; }

    public DateTime? NgayYeuCauNhanTien { get; set; }

    public Guid? MaNoiDung { get; set; }

    public Guid? MaTienTe { get; set; }

    public decimal? TyGia { get; set; }

    public decimal? SoTien { get; set; }

    public int? HinhThucChi { get; set; }

    public string? GhiChu { get; set; }

    public int? TrangThai { get; set; }

    public Guid? NguoiDuyet { get; set; }

    public DateTime? NgayDuyet { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }
}
