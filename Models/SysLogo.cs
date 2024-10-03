using System;
using System.Collections.Generic;

namespace Finance_HD.Models.HDEntity;

public partial class SysLogo
{
    public Guid Ma { get; set; }

    public string? LogoName { get; set; }

    public string? LogoImage { get; set; }

    public Guid? BranchId { get; set; }

    public int? Index { get; set; }

    public int? UserCreated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UserModified { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Deleted { get; set; }

    public int? UserDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }
}
