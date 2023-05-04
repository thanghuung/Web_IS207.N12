using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB2.Models {

    public class Inventory {

        [Key]
        public int InventoryId { get; set; }

        public int TotalAmount { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Invent_product> Invent_Products { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
    }
}