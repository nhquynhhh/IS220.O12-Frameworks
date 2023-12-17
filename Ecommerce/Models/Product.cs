using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? ProductBrandId { get; set; }

    public int? ProductCategoryId { get; set; }

    public int? ProductSubCategoryId { get; set; }

    public int? ProductOriginalPrice { get; set; }

    public int? ProductDiscountPrice { get; set; }

    public string? ProductInfo { get; set; }

    public string? ProductBarcode { get; set; }

    public int? ProductInStock { get; set; }

    public int? ProductSoldQuantity { get; set; }

    public string? ProductImage { get; set; }

    public DateTime? ProductCreatedDate { get; set; }

    public DateTime? ProductModifiedDate { get; set; }

    public bool? IsFlashSale { get; set; }

    public bool IsActive { get; set; }

    public string? ProductSlug { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Brand? ProductBrand { get; set; }

    public virtual Category? ProductCategory { get; set; }

    public virtual SubCategory? ProductSubCategory { get; set; }
}
