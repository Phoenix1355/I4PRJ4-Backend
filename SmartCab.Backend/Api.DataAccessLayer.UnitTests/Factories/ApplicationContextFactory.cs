using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Api.DataAccessLayer.UnitTests.Factories
{
    //Reference to: https://www.meziantou.net/2017/09/11/testing-ef-core-in-memory-using-sqlite
    public class ApplicationContextFactory
    {
        private string _guid;

        public ApplicationContextFactory(string guid)
        {
            _guid = guid;
            using (var context = new ApplicationContext(CreateOptions()))
            {
                context.Database.EnsureCreated();
            }
        }

        private DbContextOptions<ApplicationContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                .UseLazyLoadingProxies()
                .ConfigureWarnings(warning => warning.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInMemoryDatabase(_guid).Options;
        }

        public ApplicationContext CreateContext()
        {
                return new ApplicationContext(CreateOptions());
        }
    }
}