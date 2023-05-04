using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Ram {

        [Key]
        public int RamId { get; set; }

        public string Name { get; set; }
        public double Capacity { get; set; }
        public double MaxRam { get; set; }
        public double Speed { get; set; }
        public string Type { get; set; }
        public int Slots { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
