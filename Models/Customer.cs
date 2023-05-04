using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Customer {

        [Key]
        public int CustomerID { get; set; }

        public string UserId { get; set; }
        public string ShipAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime DateEntered { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<Voucher_detail> Voucher_Details { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}