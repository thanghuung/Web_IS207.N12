using System.Collections.Generic;

namespace WEB2.Areas.Order {

    public class ProductTrans {
        public List<int> ProductId { get; set; }
        public List<int> Quantity { get; set; }
        public int FirstInvent { get; set; }
        public int SecondInvent { get; set; }
    }
}