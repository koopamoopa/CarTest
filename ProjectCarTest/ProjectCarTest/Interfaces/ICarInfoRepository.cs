using ProjectCarTest.Dto;
using ProjectCarTest.Models;
using System.Collections.Generic;

namespace ProjectCarTest.Interfaces
{
    public interface ICarInfoRepository
    {
        IEnumerable<CarInfoDto> GetCarInfo(); // debug tool
        IEnumerable<CarInfoDto> GetCarsByUserId(int userID);
        IEnumerable<CarInfoDto> GetCarsByMake(int userID, string make);
        IEnumerable<CarInfoDto> GetCarsByModel(int userID, string model);
        IEnumerable<CarInfoDto> GetCarsByMakeAndModel(int userID, string make, string model);
        CarInfoDto AddCar(CarInfo car); 
        bool RemoveCar(int carID, int userID); 
        bool UpdateStockLevel(int carID, int userID, int newStockLevel);
    }
}
