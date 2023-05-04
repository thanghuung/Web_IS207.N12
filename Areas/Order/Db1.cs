using System.Collections.Generic;
using WEB2.Models;

namespace WEB2.Areas.Order {

    public class Db1 {
        public double Profit { get; set; }
        public int Customer { get; set; }
        public int Product { get; set; }
        public double Todei { get; set; }
        public List<OrderDetail> orderdetails { get; set; }
        public int totalfeed { get; set; }
        public int pos_feed { get; set; }
        public int dis_feed { get; set; }
        public List<string> inven_name { get; set; }
        public List<int> inven_count { get; set; }
        public List<Product> View { get; set; }
    }
}