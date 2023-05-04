using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class OS {

        [Key]
        public int OsId { get; set; }

        public string Name { get; set; }
        public string Version { get; set; }
  

        public virtual ICollection<Product> Products { get; set; }
    }
}