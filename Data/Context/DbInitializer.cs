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
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
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
        BasePrice REAL NOT NULL DEFAULT 0,
        EstimatedHours INTEGER NOT NULL DEFAULT 1
    );

    CREATE TABLE IF NOT EXISTS Roles (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT NOT NULL UNIQUE,
        Price REAL NOT NULL
    );

    CREATE TABLE IF NOT EXISTS Employees (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        FirstName TEXT NOT NULL,
        LastName TEXT NOT NULL,
        Email TEXT NOT NULL UNIQUE,
        PhoneNumber TEXT NOT NULL,
        RoleId INTEGER NOT NULL,
        FOREIGN KEY (RoleId) REFERENCES Roles (Id) ON DELETE RESTRICT
    );

    CREATE TABLE IF NOT EXISTS EmployeeService (
        EmployeeId INTEGER NOT NULL,
        ServiceId INTEGER NOT NULL,
        PRIMARY KEY (EmployeeId, ServiceId),
        FOREIGN KEY (EmployeeId) REFERENCES Employees (Id) ON DELETE CASCADE,
        FOREIGN KEY (ServiceId) REFERENCES Services (Id) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS Projects (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT NOT NULL,
        Description TEXT NOT NULL DEFAULT 'Enter workorder Description',
        StartDate TEXT NOT NULL,
        EndDate TEXT NOT NULL,
        Status TEXT NOT NULL,
        CustomerId INTEGER NOT NULL,
        ServiceId INTEGER NOT NULL,
        EmployeeId INTEGER NOT NULL,
        FOREIGN KEY (CustomerId) REFERENCES Customers (Id) ON DELETE CASCADE,
        FOREIGN KEY (ServiceId) REFERENCES Services (Id) ON DELETE CASCADE,
        FOREIGN KEY (EmployeeId) REFERENCES Employees (Id) ON DELETE CASCADE
    );";

            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
    //TestData som använder under uppgiftens gång då jag Nukat databasen vi uppstart för att testa så allt funkar rent. Låter kommentaren ligga kvar för dokumentation. 
    //public void TestData() 
    //  {
    //      using var connection = new SqliteConnection(_connectionString);
    //      connection.Open();

    //      Debug.WriteLine("Lägger till testdata...");

    //      var checkSql = "SELECT COUNT(*) FROM Customers WHERE Email = 'Sven@example.com';";
    //      using var checkCommand = new SqliteCommand(checkSql, connection);
    //      var exists = (long)checkCommand.ExecuteScalar();

    //      if (exists == 0) // Om vi inte redan har data
    //      {
    //          var sql = @"
    //      -- Skapa roller
    //      INSERT INTO Roles (Name, Price) VALUES
    //      ('Intern', 100),
    //      ('Junior', 200),
    //      ('Senior', 400);

    //      -- Skapa tjänster med BasePrice och EstimatedHours
    //      INSERT INTO Services (Name, BasePrice, EstimatedHours) VALUES
    //      ('Web Development', 500, 40),
    //      ('Database Optimization', 800, 60),
    //      ('Security Testing', 1200, 80);

    //      -- Skapa kunder
    //      INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber) VALUES
    //      ('Kalle', 'Karlsson', 'Kalle@example.se', '09000000'),
    //      ('Lisa', 'Andersson', 'Lisa@example.se', '09000001'),
    //      ('Khaled', 'Omkisson', 'Khaled@example.se', '09000002');

    //      -- Skapa anställda med roller
    //      INSERT INTO Employees (FirstName, LastName, Email, PhoneNumber, RoleId) VALUES
    //      ('Anders', 'Andersson', 'anders@example.se', '09000003', 2),
    //      ('Nils', 'Nilsson', 'nils@example.se', '09000004', 3),
    //      ('Fergie', 'Ferguson', 'fergie@example.se', '09000005', 1);
    //      ";

    //          using var command = new SqliteCommand(sql, connection);
    //          command.ExecuteNonQuery();

    //          Debug.WriteLine("Grunddata har lagts till.");

    //          // 🟢 Kolla att EmployeeServices finns innan vi lägger till data!
    //          var checkTableSql = "SELECT name FROM sqlite_master WHERE type='table' AND name='EmployeeService';";
    //          using var checkTableCommand = new SqliteCommand(checkTableSql, connection);
    //          var tableExists = checkTableCommand.ExecuteScalar();

    //          if (tableExists != null)
    //          {
    //              Debug.WriteLine("Tabellen EmployeeServices finns – lägger till relationer!");

    //              var relationSql = @"
    //          -- Koppla anställda till tjänster (EmployeeServices, Many-to-Many)
    //          INSERT INTO EmployeeService (EmployeeId, ServiceId) VALUES
    //          (1, 1), -- Anders kan jobba med Web Development
    //          (1, 2), -- Anders kan jobba med Database Optimization
    //          (2, 2), -- Nils kan jobba med Database Optimization
    //          (2, 3), -- Nils kan jobba med Security Testing
    //          (3, 3); -- Fergie kan jobba med Security Testing
    //          ";

    //              using var relationCommand = new SqliteCommand(relationSql, connection);
    //              relationCommand.ExecuteNonQuery();
    //              Debug.WriteLine("Relationer har lagts till i EmployeeServices!");
    //          }
    //          else
    //          {
    //              Debug.WriteLine("❌ ERROR: Tabellen EmployeeServices existerar INTE! Relationer kunde inte läggas till.");
    //          }

    //          // 🟢 Skapa projekt kopplade till kunder, anställda och tjänster
    //          var projectSql = @"
    //      INSERT INTO Projects (Name, Description, StartDate, EndDate, Status, CustomerId, ServiceId, EmployeeId) VALUES
    //      ('Webbshop för E-handel', 'Utveckling av en modern webbshop för e-handel', '2025-01-10', '2025-02-15', 'Ongoing', 1, 1, 2),
    //      ('Databasoptimering för Bank AB', 'Optimering av SQL-databas för bättre prestanda', '2025-02-01', '2025-03-01', 'Not Started', 2, 2, 3),
    //      ('Säkerhetstestning för FinTech', 'Genomgång och penetrationstest av FinTech-lösning', '2025-02-10', '2025-04-10', 'Completed', 3, 3, 1);
    //      ";

    //          using var projectCommand = new SqliteCommand(projectSql, connection);
    //          projectCommand.ExecuteNonQuery();

    //          Debug.WriteLine("Projekt har lagts till.");
    //      }
    //      else
    //      {
    //          Debug.WriteLine("Testdata finns redan.");
    //      }
    //  }
    //*
    //public void FetchTestData()
    //{
    //    using var connection = new SqliteConnection(_connectionString);
    //    connection.Open();

    //    var sql = @"
    //    SELECT p.Name, c.FirstName || ' ' || c.LastName AS Customer, 
    //           e.FirstName || ' ' || e.LastName AS Employee, s.Name AS Service 
    //    FROM Projects p
    //    JOIN Customers c ON p.CustomerId = c.Id
    //    JOIN Employees e ON p.EmployeeId = e.Id
    //    JOIN Services s ON p.ServiceId = s.Id;
    //";

    //    using var command = new SqliteCommand(sql, connection);
    //    using var reader = command.ExecuteReader();

    //    Debug.WriteLine("\n📝 Projekt i databasen:");
    //    while (reader.Read())
    //    {
    //        Debug.WriteLine($"Projekt: {reader.GetString(0)}");
    //        Debug.WriteLine($"Kund: {reader.GetString(1)}");
    //        Debug.WriteLine($"Anställd: {reader.GetString(2)}");
    //        Debug.WriteLine($"Tjänst: {reader.GetString(3)}");
    //        Debug.WriteLine("---------------------------");
    //    }
    //}

    public void SeedData()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        Debug.WriteLine("⚡ Initierar roller och tjänster...");

        // Kontrollera om roller redan finns
        var checkRolesSql = "SELECT COUNT(*) FROM Roles;";
        using var checkRolesCommand = new SqliteCommand(checkRolesSql, connection);
        var roleExists = (long)checkRolesCommand.ExecuteScalar();

        if (roleExists == 0)
        {
            var insertRolesSql = @"
            INSERT INTO Roles (Name, Price) VALUES
            ('Intern', 100),
            ('Junior', 200),
            ('Senior', 400);
            ";
            using var insertRolesCommand = new SqliteCommand(insertRolesSql, connection);
            insertRolesCommand.ExecuteNonQuery();
            Debug.WriteLine("✅ Roller har lagts till.");
        }
        else
        {
            Debug.WriteLine("🔹 Roller finns redan.");
        }

        // Kontrollera om tjänster redan finns
        var checkServicesSql = "SELECT COUNT(*) FROM Services;";
        using var checkServicesCommand = new SqliteCommand(checkServicesSql, connection);
        var serviceExists = (long)checkServicesCommand.ExecuteScalar();

        if (serviceExists == 0)
        {
            var insertServicesSql = @"
            INSERT INTO Services (Name, BasePrice, EstimatedHours) VALUES
            ('Web Development', 500, 40),
            ('Database Optimization', 800, 60),
            ('Security Testing', 1200, 80);
            ";
            using var insertServicesCommand = new SqliteCommand(insertServicesSql, connection);
            insertServicesCommand.ExecuteNonQuery();
            Debug.WriteLine("✅ Tjänster har lagts till.");
        }
        else
        {
            Debug.WriteLine("🔹 Tjänster finns redan.");
        }
    }

}


