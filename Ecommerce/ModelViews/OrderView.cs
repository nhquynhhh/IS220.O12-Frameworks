using Ecommerce.Models;

namespace Ecommerce.ModelViews
{
    public class OrderView
    {
        public Order DonHang { get; set; }
        public List<OrderDetail> ChiTietDonHang { get; set; }
    }
}
