using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? AccountName { get; set; }

    public string? AccountEmail { get; set; }

    public string? AccountPassword { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? AccountRegisterDate { get; set; }

    public DateTime? AccountLastLogin { get; set; }

    public int? AccountRoleId { get; set; }

    public virtual Role? AccountRole { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
