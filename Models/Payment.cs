using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB2.Models {

    public class Payment {

        [Key]
        public int PaymentId { get; set; }

        public string PaymentType { get; set; }
        public bool Allowed { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}