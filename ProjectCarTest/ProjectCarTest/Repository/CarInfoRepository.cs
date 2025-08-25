using ProjectCarTest.Data;
using ProjectCarTest.Dto;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

// Handles database interactions regarding Car Information for a specific User
namespace ProjectCarTest.Repository
{
    public class CarInfoRepository : ICarInfoRepository
    {
        private readonly DatabaseService _dbService;

        public CarInfoRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        // Debug Tool: return all cars as DTO
        public IEnumerable<CarInfoDto> GetCarInfo()
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"SELECT carID, make, model, year, stockLevel FROM CarInfos ORDER BY carID";
            return connection.Query<CarInfoDto>(sql).ToList();
        }

        // Searches for all the Cars for a specific User
        public IEnumerable<CarInfoDto> GetCarsByUserId(int userID)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"SELECT carID, make, model, year, stockLevel
                        FROM CarInfos
                        WHERE userID = @UserID
                        ORDER BY carID";
            return connection.Query<CarInfoDto>(sql, new { UserID = userID }).ToList();
        }

        // Search for specific Car, specifically only by Make
        public IEnumerable<CarInfoDto> GetCarsByMake(int userID, string make)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"SELECT carID, make, model, year, stockLevel
                        FROM CarInfos
                        WHERE userID = @UserID AND LOWER(make) = LOWER(@Make)
                        ORDER BY carID";
            return connection.Query<CarInfoDto>(sql, new { UserID = userID, Make = make }).ToList();
        }

        // Search for specific Car, specifically only by Model
        public IEnumerable<CarInfoDto> GetCarsByModel(int userID, string model)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"SELECT carID, make, model, year, stockLevel
                        FROM CarInfos
                        WHERE userID = @UserID AND LOWER(model) = LOWER(@Model)
                        ORDER BY carID";
            return connection.Query<CarInfoDto>(sql, new { UserID = userID, Model = model }).ToList();
        }

        // Search for specific Car, specifically both Make and Model
        public IEnumerable<CarInfoDto> GetCarsByMakeAndModel(int userID, string make, string model)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"SELECT carID, make, model, year, stockLevel
                        FROM CarInfos
                        WHERE userID = @UserID AND LOWER(make) = LOWER(@Make) AND LOWER(model) = LOWER(@Model)
                        ORDER BY carID";
            return connection.Query<CarInfoDto>(sql, new { UserID = userID, Make = make, Model = model }).ToList();
        }
        
        // Adds a new Car entry
        public CarInfoDto AddCar(CarInfo car)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"INSERT INTO CarInfos (userID, make, model, year, stockLevel)
                VALUES (@userID, @make, @model, @year, @stockLevel);
                SELECT last_insert_rowid();";

            var newId = connection.ExecuteScalar<int>(sql, car);

            return new CarInfoDto
            {
                CarID = newId,
                Make = car.make,
                Model = car.model,
                Year = car.year,
                StockLevel = car.stockLevel
            };
        }

        // Remove a specific Car entry
        public bool RemoveCar(int carID, int userID)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "DELETE FROM CarInfos WHERE carID = @CarID AND userID = @UserID";
            var rows = connection.Execute(sql, new { CarID = carID, UserID = userID });
            return rows > 0;
        }

        // Update the Stock Level for a specific Car
        public bool UpdateStockLevel(int carID, int userID, int newStockLevel)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "UPDATE CarInfos SET stockLevel = @StockLevel WHERE carID = @CarID AND userID = @UserID";
            var rows = connection.Execute(sql, new { StockLevel = newStockLevel, CarID = carID, UserID = userID });
            return rows > 0;
        }
    }
}
