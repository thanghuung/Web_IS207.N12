using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB2.Models {

    public class Discount {

        [Key]
        public int DiscountId { get; set; }

        public string Condition { get; set; }
        public DateTime DateRealse { get; set; }
        public DateTime DateEnd { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public double DiscountMoney { get; set; }
        public int DiscountAvailable { get; set; }
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; }
        public virtual ICollection<Gift> Gifts { get; set; }
    }
}