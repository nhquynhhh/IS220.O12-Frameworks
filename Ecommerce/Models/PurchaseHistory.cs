using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class PurchaseHistory
{
    public int HistoryId { get; set; }

    public int? CustomerId { get; set; }

    public int? OrderId { get; set; }

    public int? TotalPrice { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? PurchaseCreatedDate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Order? Order { get; set; }
}
