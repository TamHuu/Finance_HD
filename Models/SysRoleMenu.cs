using System;
using System.Collections.Generic;

namespace Finance_HD.Models;

public partial class SysRoleMenu
{
    public Guid Ma { get; set; }

    public Guid RoleId { get; set; }

    public Guid MenuId { get; set; }

    public bool? AllowAccess { get; set; }

    public bool? Status { get; set; }

    public Guid? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public Guid? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }
}
