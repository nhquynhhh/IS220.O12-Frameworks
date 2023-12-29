using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.ModelViews
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
