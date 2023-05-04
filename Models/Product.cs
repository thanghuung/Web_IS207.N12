using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB2.Models {

    public class Product {

        [Key]
        public int ProductId { get; set; }

        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int View { get; set; }
        public string Picture { get; set; }
        public double RawPrice { get; set; }
        public string ProductDetail { get; set; }
        public double MSRP { get; set; }
        public int Sold { get; set; }
        public string Version { get; set; }

        public string Color { get; set; }

        public int UnitInOrder { get; set; }
        public int ReorderLevel { get; set; }
        public int CurrentOrder { get; set; }
        public string Note { get; set; }
        public string Special { get; set; }
        public bool IsDelete { get; set; }

        //config
        public int ConnID { get; set; }

        public int ScreenID { get; set; }
        public int StructID { get; set; }
        public int SoundID { get; set; }
        public int GraphicID { get; set; }
        public int BatteryID { get; set; }
        public int RamID { get; set; }
        public int OSID { get; set; }
        public int CamID { get; set; }
        public int CPUID { get; set; }
        public int RomID { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        //config ref
        [ForeignKey("ConnID")]
        public virtual Connection Connection { get; set; }

        [ForeignKey("ScreenID")]
        public virtual Screen Screen { get; set; }

        [ForeignKey("StructID")]
        public virtual Structure Structure { get; set; }

        [ForeignKey("SoundID")]
        public virtual Sound Sound { get; set; }

        [ForeignKey("GraphicID")]
        public virtual Graphic Graphic { get; set; }

        [ForeignKey("BatteryID")]
        public virtual Battery Battery { get; set; }

        [ForeignKey("RamID")]
        public virtual Ram Ram { get; set; }

        [ForeignKey("OSID")]
        public virtual OS OS { get; set; }

        [ForeignKey("CamID")]
        public virtual Camera Camera { get; set; }

        [ForeignKey("CPUID")]
        public virtual Processor Processor { get; set; }

        [ForeignKey("RomID")]
        public virtual Rom Rom { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual ICollection<Invent_product> Invent_Products { get; set; }
        public virtual ICollection<ProductContent> ProductContents { get; set; }
    }
}