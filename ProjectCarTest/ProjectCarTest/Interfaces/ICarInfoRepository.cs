using ProjectCarTest.Dto;

namespace ProjectCarTest.Interfaces
{
    public interface ICarInfoRepository
    {
        ICollection<CarInfoDto> GetCarInfo();
        ICollection<CarInfoDto> GetCarsByUserId(int userID);
        CarInfoDto? GetCarInfoById(int carID);
        ICollection<CarInfoDto> GetCarsByMake(int userID, string make);
        ICollection<CarInfoDto> GetCarsByModel(int userID, string model);
        ICollection<CarInfoDto> GetCarsByMakeAndModel(int userID, string make, string model);
    }
}
