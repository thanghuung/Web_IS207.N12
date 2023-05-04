using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB2.Models {

    public class Voucher {

        [Key]
        public int VoucherID { get; set; }

        public string VoucherName { get; set; }
        public string VoucherDetail { get; set; }
        public int Code { get; set; }
        public bool IsActive { get; set; }
        public int Loaigiam { get; set; }
        public int Number { get; set; }
        public int Sotientoida { get; set; }
        public virtual ICollection<Voucher_detail> Voucher_Details { get; set; }
    }
}