using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.UnitTests.Factories
{
    //Reference to: https://www.meziantou.net/2017/09/11/testing-ef-core-in-memory-using-sqlite
    public class ApplicationContextFactory
    {

        public ApplicationContextFactory(DbConnection connection)
        {
            Connection = connection;
            using (var context = new ApplicationContext(CreateOptions()))
            {
                context.Database.EnsureCreated();
            }
           
        }

        public DbConnection Connection { get; private set; }

        private DbContextOptions<ApplicationContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                .UseLazyLoadingProxies()
                .UseSqlServer(Connection).Options;

        }

        public ApplicationContext CreateContext()
        {
                return new ApplicationContext(CreateOptions());
        }
    }
}