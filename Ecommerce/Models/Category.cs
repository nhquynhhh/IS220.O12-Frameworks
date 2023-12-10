using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryDescription { get; set; }

    public int? SubCategoryCount { get; set; }

    public int? ProductCount { get; set; }

    public DateTime? CategoryCreatedDate { get; set; }

    public DateTime? CategoryModifiedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}
