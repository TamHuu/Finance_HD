using System;
using System.Collections.Generic;

namespace Finance_HD.Models.HDEntity;

public partial class FaLoaiTien
{
    public Guid Ma { get; set; }

    public Guid? MaTienTe { get; set; }

    public string? Code { get; set; }

    public int? GiaTri { get; set; }

    public bool? Status { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }
}
