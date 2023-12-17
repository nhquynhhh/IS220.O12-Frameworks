using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public string? DiscountCode { get; set; }

    public string? DiscountName { get; set; }

    public string? DiscountDescription { get; set; }

    public int? DiscountQuantity { get; set; }

    public int? DiscountUsed { get; set; }

    public string? DiscountType { get; set; }

    public int? DiscountValue { get; set; }

    public bool IsActive { get; set; }

    public DateTime? DiscountStartDate { get; set; }

    public DateTime? DiscountEndDate { get; set; }

    public DateTime? DiscountCreatedDate { get; set; }

    public DateTime? DiscountModifiedDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
