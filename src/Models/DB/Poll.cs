﻿using System;
using System.Collections.Generic;

namespace SensusAPI.Models.DB;

public partial class Poll
{
    public Guid Pollid { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime ExpiryDate { get; set; }
}
