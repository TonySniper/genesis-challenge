using Antonio.TechTest.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.DataAccess.Context
{
    public class TechTestContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Order> Orders { get; set; }

        public TechTestContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TechTestContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var order = new Order
            {
                CustomerId = 10,
                DeliveryAddress = "Dummy Street 1",
                Id = 15,
                OrderStatus = OrderStatus.Completed,
                ProductId = 25,
                Quantity = 2,
                UnitPrice = 10
            };

            modelBuilder.Entity<Order>().HasData(order);

            this.ConfigureOrderMapping(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }        

        private void ConfigureOrderMapping(ModelBuilder builder)
        {
            builder.Entity<Order>().Property(x => x.ProductId).IsRequired();
            builder.Entity<Order>().Property(x => x.UnitPrice).IsRequired();
            builder.Entity<Order>().Property(x => x.Quantity).IsRequired();
            builder.Entity<Order>().Property(x => x.DeliveryAddress).IsRequired();
        }
    }
}
