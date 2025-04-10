using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoNet;
internal class Program
{
    private static void Main(string[] args)
    {
        const string connectionString = "Data Source=.;Initial Catalog=Nutshell;Integrated Security=true;TrustServerCertificate=True";
        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
    }

    private static void RegisterUser(SqlConnection connection)
    {
        SqlCommand command = new SqlCommand("RegisterUser", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Name", "Mahammad Ahmadov 2"); // existing user
        command.Parameters.AddWithValue("@Email", "mahammad@example.com");

        SqlParameter resultParameter = new SqlParameter()
        {
            ParameterName = "ResultMessage",
            SqlDbType = SqlDbType.NVarChar,
            Size = 200,
            Direction = ParameterDirection.Output,
        };
        command.Parameters.Add(resultParameter);

        command.ExecuteNonQuery();
        Console.WriteLine(resultParameter.Value);
    }

    private static void GetUsersByStatus(SqlConnection connection)
    {
        SqlCommand command = new SqlCommand("GetUsersByStatus", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add(new SqlParameter()
        {
            ParameterName = "Status",
            Value = 0
        });

        connection.Open();
        SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader.GetGuid(0)}: {reader.GetString(1)} - {reader.GetString(2)}");
        }

        reader.Close();
    }
}
