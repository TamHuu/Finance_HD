using System;
using System.Collections.Generic;

namespace Finance_HD.Models;

public partial class SysVariable
{
    public int Ma { get; set; }

    public string? Code { get; set; }

    public string Value { get; set; } = null!;
}
