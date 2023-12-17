using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string BlogTitle { get; set; } = null!;

    public string? BlogImage { get; set; }

    public string BlogSummary { get; set; } = null!;

    public string BlogContent { get; set; } = null!;

    public DateTime? BlogCreatedDate { get; set; }

    public DateTime? BlogModifiedDate { get; set; }

    public string? BlogAuthor { get; set; }

    public string? BlogSlug { get; set; }
}
