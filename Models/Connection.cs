using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Connection {

        [Key]
        public int ConnId { get; set; }

        public string Type { get; set; }
        public string WirelessCard { get; set; }
        public string Special { get; set; }
        public string Sim { get; set; }
        public string Wifi { get; set; }
        public string GPS { get; set; }
        public string Blutooth { get; set; }
        public string Other { get; set; }
        public string PhoneJack { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}