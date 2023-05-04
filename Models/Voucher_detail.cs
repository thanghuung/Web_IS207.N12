using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Voucher_detail {
        public int CustomerID { get; set; }

        public int VoucherID { get; set; }
        public int Amount { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("VoucherID")]
        public virtual Voucher Voucher { get; set; }
    }
}