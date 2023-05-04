namespace WEB2.Areas.Order {

    public class LatLong {
        public int CustomerID { get; set; }
        public int OrderId { get; set; }

        public string ShipAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}