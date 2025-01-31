using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Data.Context;

public class DbInitializer
{
    private readonly string _connectionString;

    public DbInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        Debug.WriteLine("funkar du?...");

        var sql = @"
        CREATE TABLE IF NOT EXISTS Customers (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            FirstName TEXT NOT NULL,
            LastName TEXT NOT NULL,
            Email TEXT NOT NULL UNIQUE,
            PhoneNumber TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Services (
            Id INTEGER PRIMARY KEY AUTOINCREMENT, 
            Name TEXT NOT NULL,
            Price REAL NOT NULL,
            Hours INTEGER NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Employees (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            FirstName TEXT NOT NULL,
            LastName TEXT NOT NULL,
            Email TEXT NOT NULL UNIQUE,
            PhoneNumber TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Projects (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            StartDate TEXT NOT NULL,
            EndDate TEXT NOT NULL,
            Status TEXT NOT NULL,
            TotalPrice REAL NOT NULL,
            CustomerId INTEGER NOT NULL,
            ServiceId INTEGER NOT NULL,
            EmployeeId INTEGER NOT NULL,
            FOREIGN KEY (CustomerId) REFERENCES Customers (Id) ON DELETE CASCADE,
            FOREIGN KEY (ServiceId) REFERENCES Services (Id) ON DELETE CASCADE,
            FOREIGN KEY (EmployeeId) REFERENCES Employees (Id) ON DELETE CASCADE
        );";

        using var command = new SqliteCommand(sql, connection);
        command.ExecuteNonQuery();
        Debug.WriteLine("Ja jag funkar.!");
    }



    public void TestData()  //typ hela denna är copypast från chatGPT., minus debug.Writeline för skiten inte ville funka.
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        Debug.WriteLine("Lägger till testdata...");

        // Kontrollera om det redan finns en kund med denna e-post
        var checkSql = "SELECT COUNT(*) FROM Customers WHERE Email = 'Sven@example.com';";
        using var checkCommand = new SqliteCommand(checkSql, connection);
        var exists = (long)checkCommand.ExecuteScalar(); // Hämtar antal rader med denna e-post

        if (exists == 0) // Om det inte redan finns en kund
        {
            var sql = @"    
            INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber)
            VALUES ('Sven', 'Svensson', 'Sven@example.com', '0701234567');

            INSERT INTO Employees (FirstName, LastName, Email, PhoneNumber)
            VALUES ('Kalle', 'Kallesson', 'Kalle@example.com', '0707654321');

            INSERT INTO Services (Name, Price, Hours)
            VALUES ('Stockholmsyndrom till MAUI', 500, 10);

            INSERT INTO Projects (Name, StartDate, EndDate, Status, TotalPrice, CustomerId, ServiceId, EmployeeId)
            VALUES ('Website Project', '2025-01-01', '2025-02-01', 'Ongoing', 5000, 1, 1, 1);
        ";

            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();

            Debug.WriteLine("Testdata funkar");
        }
        else
        {
            Debug.WriteLine("Testdata finns redan");
        }
    }

    public void FetchTestData()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var sql = @"
        SELECT p.Name, c.FirstName || ' ' || c.LastName AS Customer, 
               e.FirstName || ' ' || e.LastName AS Employee, s.Name AS Service 
        FROM Projects p
        JOIN Customers c ON p.CustomerId = c.Id
        JOIN Employees e ON p.EmployeeId = e.Id
        JOIN Services s ON p.ServiceId = s.Id;
    ";

        using var command = new SqliteCommand(sql, connection);
        using var reader = command.ExecuteReader();

        Debug.WriteLine("\n📝 Projekt i databasen:");
        while (reader.Read())
        {
            Debug.WriteLine($"Projekt: {reader.GetString(0)}");
            Debug.WriteLine($"Kund: {reader.GetString(1)}");
            Debug.WriteLine($"Anställd: {reader.GetString(2)}");
            Debug.WriteLine($"Tjänst: {reader.GetString(3)}");
            Debug.WriteLine("---------------------------");
        }
    }

}


