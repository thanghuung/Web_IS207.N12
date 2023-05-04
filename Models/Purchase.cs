using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Purchase {

        [Key]
        public int PurchaseId { get; set; }

        public int StaffId { get; set; }
        public int SupplierId { get; set; }

        public DateTime PurchaseDay { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactStatus { get; set; }
        public string TransactionNo { get; set; }
        public string ResponseCode { get; set; }
        public string SecureHash { get; set; }
        public double Paid { get; set; }
        public int InventoryId { get; set; }
        public DateTime DateReiceive { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}