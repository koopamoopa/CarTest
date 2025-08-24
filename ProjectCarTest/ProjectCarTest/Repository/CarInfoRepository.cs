using Microsoft.EntityFrameworkCore;
using ProjectCarTest.Dto;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;

namespace ProjectCarTest.Repository
{
    public class CarInfoRepository : ICarInfoRepository
    {
        private readonly DataContext _context;

        public CarInfoRepository(DataContext context)
        {
            _context = context;
        }

        private static CarInfoDto MapToDto(CarInfo car) =>
            new CarInfoDto
            {
                UserID = car.User.userID,
                Make = car.make,
                Model = car.model,
                Year = car.year,
                StockLevel = car.stockLevel
            };

        public ICollection<CarInfo> GetCarInfo() // Debug Tool to see entire DB
        {
            return _context.CarInfos
                .Include(c => c.User)
                .OrderBy(c => c.carID)
                .ToList();
        }

        public ICollection<CarInfoDto> GetCarsByUserId(int userID)
        {
            return _context.CarInfos
                .Include(c => c.User)
                .Where(c => c.User.userID == userID)
                .OrderBy(c => c.carID)
                .Select(c => MapToDto(c))
                .ToList();
        }

        public CarInfoDto? GetCarInfoById(int carID)
        {
            return _context.CarInfos
                .Include(c => c.User)
                .Where(c => c.carID == carID)
                .Select(c => MapToDto(c))
                .FirstOrDefault();
        }

        public ICollection<CarInfoDto> GetCarsByMake(int userID, string make)
        {
            return _context.CarInfos
                .Include(c => c.User)
                .Where(c => c.User.userID == userID && c.make.ToLower() == make.ToLower())
                .OrderBy(c => c.carID)
                .Select(c => MapToDto(c))
                .ToList();
        }

        public ICollection<CarInfoDto> GetCarsByModel(int userID, string model)
        {
            return _context.CarInfos
                .Include(c => c.User)
                .Where(c => c.User.userID == userID && c.model.ToLower() == model.ToLower())
                .OrderBy(c => c.carID)
                .Select(c => MapToDto(c))
                .ToList();
        }

        public ICollection<CarInfoDto> GetCarsByMakeAndModel(int userID, string make, string model)
        {
            return _context.CarInfos
                .Include(c => c.User)
                .Where(c => c.User.userID == userID &&
                            c.make.ToLower() == make.ToLower() &&
                            c.model.ToLower() == model.ToLower())
                .OrderBy(c => c.carID)
                .Select(c => MapToDto(c))
                .ToList();
        }

        public CarInfo? AddCar(CarInfo car)
        {
            _context.CarInfos.Add(car);
            _context.SaveChanges();
            return car;
        }

        public bool RemoveCar(int carID)
        {
            var car = _context.CarInfos.FirstOrDefault(c => c.carID == carID);
            if (car == null) return false;
            _context.CarInfos.Remove(car);
            _context.SaveChanges();
            return true;

        }

        public bool UpdateStockLevel(int carID, int newStockLevel)
        {
            var car = _context.CarInfos.FirstOrDefault(c => c.carID == carID);
            if (car == null) return false;
            car.stockLevel = newStockLevel;
            _context.SaveChanges();
            return true;

        }
    }
}
