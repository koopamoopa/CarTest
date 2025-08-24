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
        [HttpDelete("remove/{id}")]
        public IActionResult RemoveCar(int id)
        {
            var success = _carInfoRepository.RemoveCar(id);
            if (!success) return NotFound($"Car with ID {id} not found.");
            return Ok($"Car with ID {id} removed.");
        }

        // PUT: Update Stock Level
        [HttpPut("update-stock/{id}")]
        public IActionResult UpdateStockLevel(int id, [FromBody] CarInfoDto carDto)
        {
            var success = _carInfoRepository.UpdateStockLevel(id, carDto.StockLevel);
            if (!success) return NotFound($"Car with ID {id} not found.");
            return Ok($"Stock level updated to {carDto.StockLevel} for Car ID {id}.");
        }
    }
}
