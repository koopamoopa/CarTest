using Microsoft.AspNetCore.Mvc;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;
using ProjectCarTest.Dto;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

// Handles HTTP requests and responses for Car Information for a particular logged-in user via GET, POST, PUT, DELETE requests
namespace CarTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarInfoController : ControllerBase
    {
        private readonly ICarInfoRepository _carInfoRepository;

        public CarInfoController(ICarInfoRepository carInfoRepository)
        {
            _carInfoRepository = carInfoRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CarInfo>))]
        public IActionResult GetCarInfo()
        {
            var carInfos = _carInfoRepository.GetCarInfo();
            return Ok(carInfos);
        }

        [HttpGet("user")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CarInfoDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetCarsByUser()
        {
            var userID = GetUserIdFromToken();
            var cars = _carInfoRepository.GetCarsByUserId(userID);
            if (cars == null || !cars.Any()) return NotFound();
            return Ok(cars);
        }

        [HttpGet("make/{make}")]
        public IActionResult GetCarsByMake(string make)
        {
            // TODO: check the following
            // Make is within 100 char limit, english words, doesn't contain restricted characters, not null
            var userID = GetUserIdFromToken();
            var cars = _carInfoRepository.GetCarsByMake(userID, make);
            return Ok(cars);
        }

        [HttpGet("model/{model}")]
        public IActionResult GetCarsByModel(string model)
        {
            // TODO: check the following
            // Model is within 100 char limit, english words, doesn't contain restricted characters, not null
            var userID = GetUserIdFromToken();
            var cars = _carInfoRepository.GetCarsByModel(userID, model);
            return Ok(cars);
        }

        [HttpGet("make/{make}/model/{model}")]
        public IActionResult GetCarsByMakeAndModel(string make, string model)
        {
            // TODO: check the following
            // Make is within 100 char limit, english words, doesn't contain restricted characters, not null
            // Model is within 100 char limit, english words, doesn't contain restricted characters, not null
            var userID = GetUserIdFromToken();
            var cars = _carInfoRepository.GetCarsByMakeAndModel(userID, make, model);
            return Ok(cars);
        }

        // Adds a new car entry for the logged-in user
        [HttpPost("add")]
        public IActionResult AddCar([FromBody] CarCreateDto carDto)
        {
            // TODO: check if carId is within bounds
            // Make is within 100 char limit, english words, doesn't contain restricted characters, not null
            // Model is within 100 char limit, english words, doesn't contain restricted characters, not null
            // Year is between 1855-<present> (first car made in 1855, probably will never have a car with that date but never know), not null
            // Stock level is non-negative and between 1-<stock limit>

            var userID = GetUserIdFromToken();

            var car = new CarInfo
            {
                userID = userID,
                make = carDto.Make,
                model = carDto.Model,
                year = carDto.Year,
                stockLevel = carDto.StockLevel
            };

            var addedCar = _carInfoRepository.AddCar(car);

            var response = new CarInfoDto
            {
                CarID = addedCar.CarID,
                Make = addedCar.Make,
                Model = addedCar.Model,
                Year = addedCar.Year,
                StockLevel = addedCar.StockLevel
            };

            return Ok(response);
        }

        // Removes the desired car owned by the logged-in user.
        [HttpDelete("remove/{carId}")]
        public IActionResult RemoveCar(int carId)
        {
            // TODO: check if carId is within bounds (not negative, is number, not null)

            var userID = GetUserIdFromToken();
            var success = _carInfoRepository.RemoveCar(carId, userID);
            if (!success) // fails if the carID isn't owned by the user or isn't present in the database.
                return NotFound($"Car with ID {carId} for the logged-in user not found.");
            return Ok($"Car with ID {carId} removed.");
        }

        // Updates the stock level for a specific car owned by the logged-in user. 
        [HttpPut("update-stock/{carId}")]
        public IActionResult UpdateStockLevel(int carId, [FromBody] CarInfoUpdateStockDto dto)
        {
            // TODO: check if StockLevel is within bounds (not negative, is number, not null)

            var userID = GetUserIdFromToken();
            var success = _carInfoRepository.UpdateStockLevel(carId, userID, dto.StockLevel);

            if (!success) // could not update the stock level
                return NotFound($"Car with ID {carId} for the logged-in user not found.");

            // Only return the updated fields to avoid null/empty values
            var response = new
            {
                CarID = carId,
                StockLevel = dto.StockLevel
            };

            return Ok(response);
        }

        // Extract the userID from the JWT token to ensure that the specific logged-in user's data can be accessed / modified
        private int GetUserIdFromToken()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdString);
        }
    }
}
