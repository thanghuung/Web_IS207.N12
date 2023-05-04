using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Battery {

        [Key]
        public int BatteryId { get; set; }

        public int Capacity { get; set; }
        public string Type { get; set; }
        public string Technology { get; set; }
        public string Charge { get; set; }
  

        public virtual ICollection<Product> Products { get; set; }
    }
}