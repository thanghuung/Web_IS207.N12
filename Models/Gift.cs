using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Gift {

        [Key]
        public int GiftId { get; set; }

        public int DiscountId { get; set; }
        public string GiftName { get; set; }
        public int GiftAvailable { get; set; }
        public int GiftAmount { get; set; }
        public string GiftImgage { get; set; }

        [ForeignKey("DiscountId")]
        public virtual Discount ProductDiscount { get; set; }
    }
}