using System;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.UnitTests.Factories
{
    public class SqliteConnectionFactory : IDisposable
    {
        public SqliteConnectionFactory()
        {
            Connection = new SqlConnection(@"data source=.\sqlexpress;initial catalog=SmartCabDev;integrated security=true;");
            Connection.Open();
        }

        public DbConnection Connection { get; private set; }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}