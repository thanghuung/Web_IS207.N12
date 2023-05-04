using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Image {

        [Key]
        public int ImageId { get; set; }

        public int ProductId { get; set; }
        public string URL { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}