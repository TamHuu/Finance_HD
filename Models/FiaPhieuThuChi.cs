using System;
using System.Collections.Generic;

namespace Finance_HD.Models;

public partial class FiaPhieuThuChi
{
    public Guid Ma { get; set; }

    public Guid? MaLoaiThuChi { get; set; }

    public Guid? MaChiNhanhChi { get; set; }

    public Guid? MaChiNhanhThu { get; set; }

    public Guid? MaPhongBanChi { get; set; }

    public Guid? MaPhongBanThu { get; set; }

    public Guid? MaNoiDungThuChi { get; set; }

    public string? SoPhieu { get; set; }

    public DateTime? NgayLapPhieu { get; set; }

    public DateTime? NgayDuyet { get; set; }

    public Guid? NguoiDuyet { get; set; }

    public Guid? NguoiGiaoDich { get; set; }

    public string? TenNguoiGiaoDich { get; set; }

    public string? DiaChiNguoiGiaoDich { get; set; }

    public string? LyDo { get; set; }

    public Guid? MaTienTe { get; set; }

    public decimal? TyGia { get; set; }

    public int? MaHinhThuc { get; set; }

    public decimal? SoTien { get; set; }

    public string? GhiChu { get; set; }

    public int? SoHoSoKemTheo { get; set; }

    public string? MaChungTuKeToan { get; set; }

    public Guid? MaBangKeNopTien { get; set; }

    public Guid? GiamDoc { get; set; }

    public Guid? KeToanTruong { get; set; }

    public Guid? NguoiLapPhieu { get; set; }

    public Guid? NguoiChiTien { get; set; }

    public Guid? MaDeNghi { get; set; }

    public Guid? MaPhieuChi { get; set; }

    public int? TrangThai { get; set; }

    public Guid? NguoiNhanTien { get; set; }

    public string? TenNguoiNhanTien { get; set; }

    public DateTime? NgayChi { get; set; }

    public Guid? NguoiChi { get; set; }

    public bool? DaNopQuy { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public Guid? MaNganHang { get; set; }

    public Guid? MaTaiKhoanGiaoDich { get; set; }

    public string? SoTaiKhoanKhachHang { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? BillNumber { get; set; }

    public string? RequestId { get; set; }

    public bool? ThuQuaDinhDanh { get; set; }

    public string? TenTaiKhoanKhachHang { get; set; }
}
