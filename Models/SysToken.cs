using System;
using System.Collections.Generic;

namespace Finance_HD.Models.HDEntity;

public partial class SysToken
{
    public Guid Ma { get; set; }

    public Guid? UserId { get; set; }

    public string? Token { get; set; }

    public string? SignToken { get; set; }

    public string? RefreshToken { get; set; }

    public string? Ip { get; set; }

    public string? DevicesId { get; set; }

    public string? DeviceName { get; set; }

    public DateTime? LastChangeIp { get; set; }

    public DateTime? LastSeen { get; set; }

    public string? ConnectionId { get; set; }

    public bool? IsMobile { get; set; }

    public bool? Revoked { get; set; }

    public bool? Accepted { get; set; }

    public int? CreateMethod { get; set; }

    public DateTime? CreatedDate { get; set; }
}
