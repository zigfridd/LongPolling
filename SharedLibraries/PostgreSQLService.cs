using System;
using Npgsql;

namespace SharedLibraries
{
	public class PostgreSQLService : IPostgreSQLService
    {
        //NpgsqlConnection _connection;
        NpgsqlDataSource _npgsqlDataSource;

		public PostgreSQLService(string host, string user, string password, string database)
		{
            var connString = $"Host={host};Username={user};Password={password};Database={database}";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
            _npgsqlDataSource = dataSourceBuilder.Build();
            //_connection = _npgsqlDataSource.OpenConnection();
        }

        public async void AddMessage(string text, DateTime createdOn)
        {
            NpgsqlConnection connection = _npgsqlDataSource.OpenConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                await using (var cmd = new NpgsqlCommand("INSERT INTO \"WorkerMessages\" " +
                    "(\"Text\", \"CreatedOn\") VALUES (@text, @createdon)", connection))
                {
                    cmd.Parameters.AddWithValue("text", text);
                    cmd.Parameters.AddWithValue("createdon", createdOn);
                    await cmd.ExecuteNonQueryAsync();
                    connection.Close();
                }
            }
            else
                Console.WriteLine("sql not connected");
        }
    }
}

