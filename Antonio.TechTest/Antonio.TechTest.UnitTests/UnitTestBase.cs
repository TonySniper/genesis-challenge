using Antonio.TechTest.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.UnitTests
{
    public abstract class UnitTestBase
    {
        protected TechTestContext _context;

        protected UnitTestBase()
        {
            _context = this.CreateContext();
        }

        private TechTestContext CreateContext()
        {
            var randomDatabaseName = Guid.NewGuid().ToString();
            var dbOptions = new DbContextOptionsBuilder<TechTestContext>().UseInMemoryDatabase(randomDatabaseName).Options;

            return new TechTestContext(dbOptions);
        }
    }
}
