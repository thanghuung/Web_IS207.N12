using System.Collections.Generic;

namespace WEB2.Areas.Order {

    public class PurchaseItem {
        public List<int> Productid { get; set; }
        public List<int> Price { get; set; }
        public List<int> Quantity { get; set; }
        public double Paid { get; set; }
        public int SupplierId { get; set; }
        public int InvenId { get; set; }
    }
}