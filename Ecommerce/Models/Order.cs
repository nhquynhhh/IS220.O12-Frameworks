using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int? TotalPrice { get; set; }

    public int? ShippingFee { get; set; }

    public int? DiscountPrice { get; set; }

    public int? GrandPrice { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? OrderCreatedDate { get; set; }

    public DateTime? OrderCompleteDate { get; set; }

    public string? OrderAddress { get; set; }

    public string? PaymentStatus { get; set; }

    public string? OrderStatus { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<PurchaseHistory> PurchaseHistories { get; set; } = new List<PurchaseHistory>();
}
