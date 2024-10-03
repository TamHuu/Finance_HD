using System;
using System.Collections.Generic;

namespace Finance_HD.Models;

public partial class FiaBdsdnganHang
{
    public Guid Ma { get; set; }

    public Guid? MaNganHang { get; set; }

    public string? NoiDung { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Error { get; set; }

    public bool? Test { get; set; }

    public string? RequestId { get; set; }

    public string? Request { get; set; }
}
