using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class SubCategory
{
    public int SubCategoryId { get; set; }

    public string? SubCategoryName { get; set; }

    public string? SubCategoryDescription { get; set; }

    public int? ProductCount { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? SubCategoryCreatedDate { get; set; }

    public DateTime? SubCategoryModifiedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
