using System;
using System.Collections.Generic;

namespace SensusAPI.Models.DB;

public partial class Question
{
    public Guid Questionid { get; set; }

    public Guid? Pollid { get; set; }

    public string Question1 { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Poll? Poll { get; set; }
}
