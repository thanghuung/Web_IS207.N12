using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WEB2.Models;

namespace WEB2.Data {

    public class AppDbContext : IdentityDbContext<AppUser> {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            // Bỏ tiền tố AspNet của các bảng: mặc định
            foreach (var entityType in builder.Model.GetEntityTypes()) {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet")) {
                    entityType.SetTableName(tableName[6..]);
                }
            }
            builder.Entity<Voucher_detail>().HasKey(p => new { p.CustomerID, p.VoucherID });
            builder.Entity<OrderDetail>().HasKey(p => new { p.OrderId, p.ProductId });
            builder.Entity<PurchaseDetail>().HasKey(p => new { p.PurchaseId, p.ProductId });
            builder.Entity<ProductDiscount>().HasKey(p => new { p.ProductId, p.DiscountId });
            builder.Entity<Invent_product>().HasKey(p => new { p.ProductId, p.InventoryId });
            builder.Entity<ProductContent>().HasKey(p => new { p.ProductId, p.ContentId });

            builder.Entity<ChatUser>()
                .HasKey(x => new { x.ChatId, x.UserId });
        }

        public DbSet<WEB2.Models.Staff> Staff { get; set; }
        public DbSet<WEB2.Models.ProductDiscount> ProductDiscount { get; set; }
        public DbSet<WEB2.Models.Discount> Discounts { get; set; }
        public DbSet<WEB2.Models.Category> Category { get; set; }

        public DbSet<WEB2.Models.Battery> Battery { get; set; }

        public DbSet<WEB2.Models.Customer> Customer { get; set; }

        public DbSet<WEB2.Models.Order> Order { get; set; }

        public DbSet<WEB2.Models.Calendar> Calendar { get; set; }

        public DbSet<WEB2.Models.Supplier> Supplier { get; set; }

        public DbSet<WEB2.Models.Inventory> Inventory { get; set; }

        public DbSet<WEB2.Models.Shipment> Shipment { get; set; }

        public DbSet<WEB2.Models.Voucher> Voucher { get; set; }

        public DbSet<WEB2.Models.Product> Product { get; set; }
        public DbSet<WEB2.Models.Content> Content { get; set; }
        public DbSet<WEB2.Models.ProductContent> ProductContent { get; set; }
        public DbSet<WEB2.Models.Ram> Ram { get; set; }

        public DbSet<WEB2.Models.OrderDetail> OrderDetail { get; set; }
        public DbSet<WEB2.Models.Payment> Payment { get; set; }
        public DbSet<WEB2.Models.Voucher_detail> Voucher_Details { get; set; }
        public DbSet<WEB2.Models.Invent_product> Invent_Product { get; set; }
        public DbSet<WEB2.Models.PurchaseDetail> PurchaseDetail { get; set; }
        public DbSet<WEB2.Models.Purchase> Purchase { get; set; }
        public DbSet<WEB2.Models.Feedback> Feedback { get; set; }

        public DbSet<WEB2.Models.Chat> Chats { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }

        public DbSet<WEB2.Models.Message> Messages { get; set; }
    }
}