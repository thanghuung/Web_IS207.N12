using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Staff {

        [Key]
        public int StaffId { get; set; }

        public string UserId { get; set; }
        public DateTime WorkingDay { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}