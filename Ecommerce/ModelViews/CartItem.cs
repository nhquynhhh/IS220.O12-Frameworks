using System;
using Ecommerce.Models;

namespace Ecommerce.ModelViews
{
    public class CartItem
    {

        public Product product { get; set; }
        public int amount { get; set; }
        public double TotalMoney => amount * product.ProductDiscountPrice.Value;
    }
}
