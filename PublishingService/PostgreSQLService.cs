using System;
using Npgsql;

namespace PublishingService
{
	public class PostgreSQLService : IPostgreSQLService
    {
        NpgsqlConnection _connection;

		public PostgreSQLService(NpgsqlConnection connection)
		{
            _connection = connection;
		}

        public async void AddMessage(string text, DateTime createdOn)
        {
            if(_connection.State == System.Data.ConnectionState.Open)
            {
                
                await using (var cmd = new NpgsqlCommand("INSERT INTO \"WorkerMessages\" " +
                    "(\"Text\", \"CreatedOn\") VALUES (@text, @createdon)", _connection))
                {
                    cmd.Parameters.AddWithValue("text", text);
                    cmd.Parameters.AddWithValue("createdon", createdOn);
                    await cmd.ExecuteNonQueryAsync();
                }
                
                /*Console.WriteLine();
                await using (var cmd = new NpgsqlCommand("SELECT Text FROM workermessages", _connection))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        Console.WriteLine(reader.GetString(0));
                }*/
            }
            else
                Console.WriteLine("sql not connected");
        }
    }
}

