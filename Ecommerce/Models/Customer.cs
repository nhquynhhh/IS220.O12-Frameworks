using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int? AccountId { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerEmail { get; set; }

    public string? CustomerPhone { get; set; }

    public string? CustomerAddress { get; set; }

    public string? CustomerAvatar { get; set; }

    public DateTime? CustomerJoinDate { get; set; }

    public int? CustomerOrderQuantity { get; set; }

    public string? CustomerBankAccount { get; set; }

    public string? CustomerBank { get; set; }

    public bool? IsActive { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PurchaseHistory> PurchaseHistories { get; set; } = new List<PurchaseHistory>();
}
