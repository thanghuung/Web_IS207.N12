using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Invent_product {
        public int ProductId { get; set; }
        public int InventoryId { get; set; }
        public int ProductAvailable { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("InventoryId")]
        public virtual Inventory Inventory { get; set; }
    }
}