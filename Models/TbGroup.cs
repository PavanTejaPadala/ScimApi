using System;
using System.Collections.Generic;

namespace ScimApi.Models;

public partial class TbGroup
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public bool? Active { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastModifiedDate { get; set; }
}
