using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Sound {

        [Key]
        public int SoundId { get; set; }

        public string Technology { get; set; }


        public virtual ICollection<Product> Products { get; set; }
    }
}