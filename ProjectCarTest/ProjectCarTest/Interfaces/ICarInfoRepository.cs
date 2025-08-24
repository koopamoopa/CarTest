using ProjectCarTest.Dto;
using ProjectCarTest.Models;

namespace ProjectCarTest.Interfaces
{
    public interface ICarInfoRepository
    {
        ICollection<CarInfo> GetCarInfo();
        ICollection<CarInfoDto> GetCarsByUserId(int userID);
        CarInfoDto? GetCarInfoById(int carID);
        ICollection<CarInfoDto> GetCarsByMake(int userID, string make);
        ICollection<CarInfoDto> GetCarsByModel(int userID, string model);
        ICollection<CarInfoDto> GetCarsByMakeAndModel(int userID, string make, string model);
        CarInfo? AddCar(CarInfo car);
        bool RemoveCar(int carID);
        bool UpdateStockLevel(int carID, int newStockLevel);

    }
}
