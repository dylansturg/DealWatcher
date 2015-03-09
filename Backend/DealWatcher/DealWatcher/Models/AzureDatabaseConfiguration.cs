using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace DealWatcher.Models
{
    public class AzureDatabaseConfiguration : DbConfiguration
    {
        public AzureDatabaseConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy(2, TimeSpan.FromSeconds(30)));
        }
    }
}