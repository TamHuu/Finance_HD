using System;
using System.Collections.Generic;

namespace Finance_HD.Models;

public partial class FiaChiTietBangKeNhanVien
{
    public Guid Ma { get; set; }

    public Guid? MaBangKe { get; set; }

    public Guid? MaNhanVien { get; set; }

    public decimal? SoTien { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }
}
