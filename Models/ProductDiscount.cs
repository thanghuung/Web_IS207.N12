using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class ProductDiscount {
        public int ProductId { get; set; }
        public int DiscountId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("DiscountId")]
        public virtual Discount Discount { get; set; }
    }
}