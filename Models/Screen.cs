using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Screen {

        [Key]
        public int ScreenId { get; set; }

        public string Resolution { get; set; }
        public string Size { get; set; }
        public string HZ { get; set; }
        public string MaxBright { get; set; }
        public string Special { get; set; }
        public string Technology { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}