using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class OrderDetail {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public string IDSKU { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public bool Fulfilled { get; set; }
        public string Voucher { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}