using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Content {

        [Key]
        public int ContentId { get; set; }

        public string Title { get; set; }
        public string Contents { get; set; }
        public DateTime DateRealease { get; set; }
        public string Author { get; set; }
        public string Special { get; set; }
        public virtual ICollection<ProductContent> ProductContents { get; set; }
    }
}