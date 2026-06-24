using Microsoft.Data.Sqlite;

namespace SafeVault.Web.Data;


public class UserSearchRepository
{
    private readonly string _connectionString;

    public UserSearchRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void EnsureSchemaAndSeedData()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var createCmd = connection.CreateCommand();
        createCmd.CommandText =
            "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, Username TEXT NOT NULL, Email TEXT NOT NULL);";
        createCmd.ExecuteNonQuery();

        using var countCmd = connection.CreateCommand();
        countCmd.CommandText = "SELECT COUNT(*) FROM Users;";
        var count = (long)(countCmd.ExecuteScalar() ?? 0L);
        if (count == 0)
        {
            using var seedCmd = connection.CreateCommand();
            seedCmd.CommandText =
                "INSERT INTO Users (Username, Email) VALUES (@u1, @e1), (@u2, @e2);";
            seedCmd.Parameters.AddWithValue("@u1", "alice");
            seedCmd.Parameters.AddWithValue("@e1", "alice@example.com");
            seedCmd.Parameters.AddWithValue("@u2", "bob");
            seedCmd.Parameters.AddWithValue("@e2", "bob@example.com");
            seedCmd.ExecuteNonQuery();
        }
    }

    public List<(int Id, string Username, string Email)> SearchUsersByUsername(string term)
    {
        var results = new List<(int Id, string Username, string Email)>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Username, Email FROM Users WHERE Username = @term;";
        cmd.Parameters.AddWithValue("@term", term);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            results.Add((reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
        }

        return results;
    }
}
