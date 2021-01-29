using System;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;

namespace CassandraClient
{
    internal class Program
    {
        private static async Task Main()
        {
            var cluster = Cluster.Builder()
                .AddContactPoints("localhost")
                .Build();
            var session = await cluster.ConnectAsync("root");

            while (true)
            {
                var query = $"INSERT  INTO  root.employee(id,name) VALUES (uuid(),'Elyor Latipov')";
                var statement = new SimpleStatement(query);
                 
                await session.ExecuteAsync(statement).ContinueWith(async task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        var rows = await session.ExecuteAsync(new SimpleStatement("SELECT count(*) as total FROM employee"));
                        var row = rows.FirstOrDefault();
                     
                        if (row != null)
                            Console.WriteLine(row.GetValue<long>("total"));

                    }
                });

            }
        }
    }
}
