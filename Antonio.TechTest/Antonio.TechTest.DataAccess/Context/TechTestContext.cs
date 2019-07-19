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
        }
    }
}
