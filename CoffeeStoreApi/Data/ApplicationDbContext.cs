using System;
using System.Reflection.Emit;
using CoffeeStoreApi.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoffeeStoreApi.Models
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
	  public	DbSet<Product> Products { set; get; }

	  public	DbSet<OrderItem> orderItems { set; get; }

	  public	DbSet<Order> Orders { set; get; }

      public  DbSet<RefreshToken> refreshTokens { set; get; }

      public DbSet<Category> Categories { set; get; }

      public DbSet<DeliveryInformation> Deliveries { set; get; }

      public DbSet<Payment> Payments { set; get; }
 
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItem>()
         .HasKey(oi => new { oi.OrderId, oi.ProductId });

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);
        }
    }
}

