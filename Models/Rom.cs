using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Rom {

        [Key]
        public int RomId { get; set; }

        public int Capacity { get; set; }
        public int MaxRom { get; set; }
        public string Type { get; set; }


        public virtual ICollection<Product> Products { get; set; }
    }
}