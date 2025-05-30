using System.IO;
using Npgsql;
using static System.Environment;


public class Program
{
    public static void Main()
    {
        // Console.WriteLine($"Special folder: {Directory.GetCurrentDirectory()}");
        string DbFolder = $"{GetFolderPath(SpecialFolder.Personal)}";
        // string currentFolder = Directory.GetCurrentDirectory();
        Console.WriteLine($"Nome da pasta: {DbFolder}");

        string connectionString = $"Host=localhost;Port=5432;Username=admin;Password=1234;Database=stripepayments;";

        try
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                ExecuteScript(connection, File.ReadAllText($"{DbFolder}/Developer/C#/Maggie/PostgresScript/coupleGame.sql"));

                Console.WriteLine("Database created with success!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro ao tentar conectar");
            Console.WriteLine(e);
        }
    }

    static void ExecuteScript(NpgsqlConnection connection, string script)
    {
        using (NpgsqlCommand command = new NpgsqlCommand(script, connection))
        {
            command.ExecuteNonQuery();
        }
    }
}