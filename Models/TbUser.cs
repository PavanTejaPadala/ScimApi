using System;
using System.Collections.Generic;

namespace ScimApi.Models;

public partial class TbUser
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? DisplayName { get; set; }

    public string? EmailAddress { get; set; }

    public bool? Enabled { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastModifiedDate { get; set; }
}
