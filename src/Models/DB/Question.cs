using System;
using System.Collections.Generic;

namespace SensusAPI.Models.DB;

public partial class Question
{
    public Guid Questionid { get; set; }

    public string PollId { get; set; } = null!;
}
