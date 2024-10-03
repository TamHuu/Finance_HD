using System;
using System.Collections.Generic;

namespace Finance_HD.Models;

public partial class SysStt
{
    public Guid Ma { get; set; }

    public string? TableName { get; set; }

    public string? Colname { get; set; }

    public decimal? TableMax { get; set; }

    public DateTime? CreateDate { get; set; }
}
