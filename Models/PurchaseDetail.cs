using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class PurchaseDetail {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public string IDSKU { get; set; }
        public double Total { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("PurchaseId")]
        public virtual Purchase Purchase { get; set; }
    }
}