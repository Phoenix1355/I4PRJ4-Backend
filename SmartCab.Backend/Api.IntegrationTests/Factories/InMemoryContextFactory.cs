using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.UnitTests.Factories
{
    //Reference to: https://www.meziantou.net/2017/09/11/testing-ef-core-in-memory-using-sqlite
    public class InMemoryContextFactory : IDisposable
    {
        private string _guid;

        public InMemoryContextFactory(string guid)
        {
            _guid = guid;
        }


        private DbContextOptions<ApplicationContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                .UseLazyLoadingProxies()
                .UseInMemoryDatabase(_guid).Options;

        }

        public ApplicationContext CreateContext()
        {
          using (var context = new ApplicationContext(CreateOptions()))
                {
                    context.Database.EnsureCreated();
                }
            

            return new ApplicationContext(CreateOptions());
        }

        public void Dispose()
        {

        }
    }
}