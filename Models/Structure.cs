using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Structure {

        [Key]
        public int StructId { get; set; }

        public double Wide { get; set; }
        public double Weight { get; set; }
        public double High { get; set; }
        public double Long { get; set; }
        public string Martirial { get; set; }
        public string Design { get; set; }


        public virtual ICollection<Product> Products { get; set; }
    }
}