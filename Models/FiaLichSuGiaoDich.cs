using System;
using System.Collections.Generic;

namespace Finance_HD.Models.HDEntity;

public partial class FiaLichSuGiaoDich
{
    public Guid Ma { get; set; }

    public Guid? MaNganHang { get; set; }

    public string? MaGiaoDich { get; set; }

    public DateTime? NgayGiaoDich { get; set; }

    public string? SoTaiKhoanDoiUng { get; set; }

    public string? TenTaiKhoanDoiUng { get; set; }

    public string? TenNganHangDoiUng { get; set; }

    public string? LoaiTien { get; set; }

    public decimal? SoTien { get; set; }

    public decimal? SoTienSauGiaoDich { get; set; }

    public string? GhiChu { get; set; }

    public string? MaKenhThanhToan { get; set; }

    public string? SoThamChieuKenh { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public string? SoTaiKhoanTruyVan { get; set; }

    public string? LoaiGiaoDich { get; set; }
}
