using System;
using System.Collections.Generic;

namespace Finance_HD.Models.HDEntity;

public partial class SysMenu
{
    public Guid Ma { get; set; }

    public string? Code { get; set; }

    public Guid? ParentId { get; set; }

    public string? Name { get; set; }

    public int? Sequence { get; set; }

    public string? Link { get; set; }

    public string? Icon { get; set; }

    public string? ClassIcon { get; set; }

    public Guid? ChildOfMenu { get; set; }

    public Guid? NhomGiaoDien { get; set; }

    public Guid? MaPhongKham { get; set; }

    public bool? Target { get; set; }

    public bool? Status { get; set; }

    public string? UsingFor { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }
}
