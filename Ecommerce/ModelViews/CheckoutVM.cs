using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ModelViews
{
    public class CheckoutVM
    {
        public int CustomerId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
        
        public string Note { get; set; }
    }
}
