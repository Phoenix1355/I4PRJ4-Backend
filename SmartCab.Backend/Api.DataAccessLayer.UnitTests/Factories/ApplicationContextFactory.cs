using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace Api.DataAccessLayer.UnitTests.Factories
{
    //Reference to: https://www.meziantou.net/2017/09/11/testing-ef-core-in-memory-using-sqlite
    public class ApplicationContextFactory : IDisposable
    {
        private DbConnection _connection;

        private DbContextOptions<ApplicationContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                .UseLazyLoadingProxies()
                .UseSqlite(_connection).Options;

        }

        public ApplicationContext CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                using (var context = new ApplicationContext(CreateOptions()))
                {
                    context.Database.EnsureCreated();
                }
            }

            return new ApplicationContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
