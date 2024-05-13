using System;
using System.Collections.Generic;

namespace SensusAPI.Models.DB;

public partial class Answer
{
    public Guid Answerid { get; set; }

    public Guid Questionid { get; set; }

    public Guid Pollid { get; set; }

    public string Ans { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual Poll Poll { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;
}
