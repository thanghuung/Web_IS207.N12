using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Processor {

        [Key]
        public int CpuId { get; set; }

        public string Name { get; set; }
        public string Genth { get; set; }
        public int Core { get; set; }
        public int Thread { get; set; }
        public double BaseSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public double Cache { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}