using System;

namespace WEB2.Areas.Order {

    public class PurchaseOrder {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Paid { get; set; }
        public string Quantity { get; set; }
        public string Type { get; set; }
        public DateTime DayTransaction { get; set; }
        public string Fullname { get; set; }
        public int InvenId { get; set; }

        public PurchaseOrder() {
        }
    }
}