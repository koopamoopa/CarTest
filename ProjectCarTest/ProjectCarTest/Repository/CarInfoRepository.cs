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

        // Helper to map entity -> DTO
        private static CarInfoDto MapToDto(CarInfo car) =>
            new CarInfoDto
            {
                Make = car.make,
                Model = car.model,
                Year = car.year,
                StockLevel = car.stockLevel
            };

        public ICollection<CarInfoDto> GetCarInfo()
        {
            return _context.CarInfos
                .OrderBy(c => c.carID)
                .Select(c => MapToDto(c))
                .ToList();
        }

        public ICollection<CarInfoDto> GetCarsByUserId(int userID)
        {
            return _context.CarInfos
                .Where(c => c.userID == userID)
                .OrderBy(c => c.carID)
                .Select(c => MapToDto(c))
                .ToList();
        }

        public CarInfoDto? GetCarInfoById(int carID)
        {
            return _context.CarInfos
                .Where(c => c.carID == carID)
                .Select(c => MapToDto(c))
                .FirstOrDefault();
        }

        public ICollection<CarInfoDto> GetCarsByMake(int userID, string make)
        {
            return _context.CarInfos
                .Where(c => c.userID == userID && c.make.ToLower() == make.ToLower())
                .OrderBy(c => c.carID)
                .Select(c => MapToDto(c))
                .ToList();
        }

        public ICollection<CarInfoDto> GetCarsByModel(int userID, string model)
        {
            return _context.CarInfos
                .Where(c => c.userID == userID && c.model.ToLower() == model.ToLower())
                .OrderBy(c => c.carID)
                .Select(c => MapToDto(c))
                .ToList();
        }

        public ICollection<CarInfoDto> GetCarsByMakeAndModel(int userID, string make, string model)
        {
            return _context.CarInfos
                .Where(c => c.userID == userID &&
                            c.make.ToLower() == make.ToLower() &&
                            c.model.ToLower() == model.ToLower())
                .OrderBy(c => c.carID)
                .Select(c => MapToDto(c))
                .ToList();
        }
    }
}
