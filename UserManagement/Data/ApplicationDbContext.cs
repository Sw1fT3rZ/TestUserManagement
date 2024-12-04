using Microsoft.EntityFrameworkCore;
using UserManagementApp.Models;

namespace UserManagementApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Таблицы базы данных
        public DbSet<User> Users { get; set; } // Таблица пользователей
        public DbSet<Product> Products { get; set; } // Таблица продуктов
        public DbSet<Address> Addresses { get; set; } // Таблица адресов
        public DbSet<Order> Orders { get; set; } // Таблица заказов
        public DbSet<OrderItem> OrderItems { get; set; } // Таблица элементов заказа
        public DbSet<Payment> Payments { get; set; } // Таблица оплат

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address) 
                .WithMany() 
                .HasForeignKey(o => o.AddressId) 
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order) 
                .WithOne(o => o.Payment) 
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Order>()
                .Property(o => o.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
