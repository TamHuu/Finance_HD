using System;
using System.Collections.Generic;

namespace Finance_HD.Models;

public partial class FiaTaiKhoanNganHang
{
    public Guid Ma { get; set; }

    public Guid? MaNganHang { get; set; }

    public Guid? MaLoai { get; set; }

    public Guid? MaTienTe { get; set; }

    public string? SoTaiKhoan { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public bool? Status { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public string? DienGiai { get; set; }

    public Guid? DongTienThu { get; set; }

    public Guid? DongTienChi { get; set; }

    public Guid? MaChiNhanh { get; set; }

    public Guid? MaPhongBan { get; set; }

    public string? Pid { get; set; }

    public string? Mid { get; set; }
}
