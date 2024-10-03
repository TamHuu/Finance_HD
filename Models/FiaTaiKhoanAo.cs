using System;
using System.Collections.Generic;

namespace Finance_HD.Models;

public partial class FiaTaiKhoanAo
{
    public Guid Ma { get; set; }

    public Guid? MaNganHang { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public string? Code { get; set; }

    public string? Ten { get; set; }

    public bool? Status { get; set; }

    public Guid? MaTaiKhoanLienKet { get; set; }

    public string? Vacode { get; set; }
}
