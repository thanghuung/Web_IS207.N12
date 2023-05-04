using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Category {

        [Key]
        public int CategoryId { get; set; }

        // Category cha (FKey)
        public int? ParentCategoryId { get; set; }

        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string Picture { get; set; }

        [ForeignKey("ParentCategoryId")]
        public Category ParentCategory { set; get; }

        public virtual ICollection<Product> Product { get; set; }
    }
}