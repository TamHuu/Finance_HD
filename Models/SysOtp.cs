using System;
using System.Collections.Generic;

namespace Finance_HD.Models.HDEntity;

public partial class SysOtp
{
    public Guid Ma { get; set; }

    public Guid? MaUser { get; set; }

    public Guid? MaToken { get; set; }

    public string? Otp { get; set; }

    public DateTime? CreatedDate { get; set; }
}
