using Microsoft.AspNetCore.Mvc;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;
using ProjectCarTest.Dto;

namespace CarTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarInfoController : Controller
    {
        private readonly ICarInfoRepository _carInfoRepository;
        private readonly DataContext _context;

        public CarInfoController(ICarInfoRepository carInfoRepository, DataContext context)
        {
            _carInfoRepository = carInfoRepository;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CarInfo>))]
        public IActionResult GetCarInfo()
        {
            var carInfos = _carInfoRepository.GetCarInfo();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(carInfos);
        }

        [HttpGet("user/{userID}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CarInfo>))]
        [ProducesResponseType(404)]
        public IActionResult GetCarsByUserId(int userID)
        {
            var cars = _carInfoRepository.GetCarsByUserId(userID);
            if (cars == null || !cars.Any()) return NotFound();
            return Ok(cars);
        }

        [HttpGet("user/{userID}/make/{make}")]
        public IActionResult GetCarsByMake(int userId, string make)
        {
            var cars = _carInfoRepository.GetCarsByMake(userId, make);
            return Ok(cars);
        }

        [HttpGet("user/{userID}/model/{model}")]
        public IActionResult GetCarsByModel(int userId, string model)
        {
            var cars = _carInfoRepository.GetCarsByModel(userId, model);
            return Ok(cars);
        }

        [HttpGet("user/{userID}/make/{make}/model/{model}")]
        public IActionResult GetCarsByMakeAndModel(int userID, string make, string model)
        {
            var cars = _carInfoRepository.GetCarsByMakeAndModel(userID, make, model);
            return Ok(cars);
        }

        // POST: Add Car
        [HttpPost("add")]
        public IActionResult AddCar([FromBody] CarInfoDto carDto)
        {
            var user = _context.Users.Find(carDto.UserID);
            if (user == null) return NotFound($"User with ID {carDto.UserID} not found.");

            var car = new CarInfo
            {
                User = user,
                make = carDto.Make,
                model = carDto.Model,
                year = carDto.Year,
                stockLevel = carDto.StockLevel
            };

            var addedCar = _carInfoRepository.AddCar(car);
            return Ok(addedCar);
        }

        // DELETE: Remove Car
        [HttpDelete("user/{userId}/remove/{carId}")]
        public IActionResult RemoveCar(int userId, int carId)
        {
            // Check if the car belongs to the specified user
            var car = _context.CarInfos.FirstOrDefault(c => c.carID == carId && c.userID == userId);
            if (car == null)
                return NotFound($"Car with ID {carId} for User ID {userId} not found.");

            _context.CarInfos.Remove(car);
            _context.SaveChanges();

            return Ok($"Car with ID {carId} for User ID {userId} removed.");
        }


        // PUT: Update Stock Level
        [HttpPut("user/{userId}/update-stock/{carId}")]
        public IActionResult UpdateStockLevel(int userId, int carId, [FromBody] CarInfoUpdateStockDto dto)
        {
            // First, check if the car belongs to the user
            var car = _context.CarInfos.FirstOrDefault(c => c.carID == carId && c.userID == userId);
            if (car == null)
                return NotFound($"Car with ID {carId} for User ID {userId} not found.");

            // Update stock level
            car.stockLevel = dto.StockLevel;
            _context.SaveChanges();

            return Ok(car); // Return updated car for verification
        }

    }
}
