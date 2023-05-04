using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class ProductContent {
        public int ProductId { get; set; }
        public int ContentId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}