using System;
using System.Collections.Generic;

namespace Finance_HD.Models;

public partial class VPermission
{
    public Guid RoleId { get; set; }

    public Guid MenuId { get; set; }

    public Guid PermissionId { get; set; }

    public Guid UserId { get; set; }

    public string? Username { get; set; }

    public bool EmailReceive { get; set; }

    public string? FullName { get; set; }

    public bool? AllowAccess { get; set; }

    public Guid? BranchId { get; set; }
}
