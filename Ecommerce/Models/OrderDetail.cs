using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? ProductPrice { get; set; }

    public int? ProductQuantity { get; set; }

    public int? ProductTotalPrice { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
