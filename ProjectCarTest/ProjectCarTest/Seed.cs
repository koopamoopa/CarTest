using ProjectCarTest.Models;
using ProjectCarTest.Data;
using Dapper;

namespace ProjectCarTest.Data
{
    public class Seed
    {
        private readonly DatabaseService _dbService;

        public Seed(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public void Initialize()
        {
            using var connection = _dbService.CreateConnection();

            // Create tables if they don't exist
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Users (
                    userID INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL,
                    password TEXT NOT NULL,
                    companyName TEXT
                );
                
                CREATE TABLE IF NOT EXISTS CarInfos (
                    carID INTEGER PRIMARY KEY AUTOINCREMENT,
                    userID INTEGER NOT NULL,
                    make TEXT NOT NULL,
                    model TEXT NOT NULL,
                    year INTEGER NOT NULL,
                    stockLevel INTEGER NOT NULL,
                    FOREIGN KEY (userID) REFERENCES Users(userID) ON DELETE CASCADE
                );
            ");

            // Check if users exist
            var userCount = connection.ExecuteScalar<int>("SELECT COUNT(1) FROM Users");
            if (userCount > 0) return; // already seeded

            // Seed users
            var user1Id = connection.ExecuteScalar<int>(
                "INSERT INTO Users (username, password, companyName) VALUES (@Username, @Password, @CompanyName); SELECT last_insert_rowid();",
                new { Username = "audi_owner", Password = "Pass1234$", CompanyName = "Audi Lovers Inc." });

            var user2Id = connection.ExecuteScalar<int>(
                "INSERT INTO Users (username, password, companyName) VALUES (@Username, @Password, @CompanyName); SELECT last_insert_rowid();",
                new { Username = "thebmw", Password = "b7M#@w13", CompanyName = "BMW Enthusiasts Ltd." }); // 8 character password

            var user3Id = connection.ExecuteScalar<int>(
                "INSERT INTO Users (username, password, companyName) VALUES (@Username, @Password, @CompanyName); SELECT last_insert_rowid();",
                new { Username = "mercWork", Password = "YEeCsB#&3ggsd$XYyeNsnqPHAa?5qm", CompanyName = "Mercedes Fans Co." }); // 30 character password

            // Seed cars
            connection.Execute(
                "INSERT INTO CarInfos (userID, make, model, year, stockLevel) VALUES (@UserID, @Make, @Model, @Year, @StockLevel);",
                new[]
                {
                    new { UserID = user1Id, Make = "Audi", Model = "A4", Year = 2018, StockLevel = 3 },
                    new { UserID = user1Id, Make = "Audi", Model = "Q5", Year = 2019, StockLevel = 4 },
                    new { UserID = user2Id, Make = "BMW", Model = "X5", Year = 2020, StockLevel = 5 },
                    new { UserID = user2Id, Make = "BMW", Model = "X6", Year = 2013, StockLevel = 4 },
                    new { UserID = user3Id, Make = "Mercedes", Model = "C-Class", Year = 2021, StockLevel = 2 },
                    new { UserID = user3Id, Make = "Mercedes-Benz", Model = "A-Class", Year = 2019, StockLevel = 7 },
                    new { UserID = user1Id, Make = "Audi", Model = "A6", Year = 2020, StockLevel = 2 }
                });
        }
    }
}
