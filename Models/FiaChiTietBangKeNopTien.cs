using System;
using System.Collections.Generic;

namespace Finance_HD.Models.HDEntity;

public partial class FiaChiTietBangKeNopTien
{
    public Guid Ma { get; set; }

    public Guid? MaBangKeNopTien { get; set; }

    public Guid? MaLoaiTien { get; set; }

    public int? SoLuong { get; set; }

    public decimal? ThanhTien { get; set; }

    public string? GhiChu { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }
}
