using System;
using System.Collections.Generic;

namespace Finance_HD.Models.HDEntity;

public partial class SysUser
{
    public Guid Ma { get; set; }

    public Guid? BranchId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public Guid? MaPhongBan { get; set; }

    public Guid? MaPhongBanKiemNhiem { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public Guid? MaChucDanh { get; set; }

    public Guid? MaChucVu { get; set; }

    public int? GioiTinh { get; set; }

    public string? HinhAnh { get; set; }

    public string? Msnv { get; set; }

    public DateTime? NgayVaoLam { get; set; }

    public DateTime? NgayKetThuc { get; set; }

    public string? MaDinhDanh { get; set; }

    public string? Cccd { get; set; }

    public bool? EmailReceive { get; set; }

    public string? Fcm { get; set; }

    public bool? Status { get; set; }

    public Guid? MaKhuVuc { get; set; }

    public string? KhuVuc { get; set; }

    public string? QuayTiepNhan { get; set; }

    public string? MaPhongKhamChucNang { get; set; }

    public string? PhongKhamDieuTri { get; set; }

    public bool? GroupManager { get; set; }

    public bool? UserManager { get; set; }

    public bool? LdapLogin { get; set; }

    public string? Cchn { get; set; }

    public string? LinkAnhCkdt { get; set; }

    public string? LinkAnhCks { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }
}
