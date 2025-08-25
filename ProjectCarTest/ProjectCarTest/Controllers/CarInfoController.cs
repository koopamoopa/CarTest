using Microsoft.AspNetCore.Mvc;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;
using ProjectCarTest.Dto;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using static ProjectCarTest.Utilities.Helper;

// Handles HTTP requests and responses for Car Information for a particular logged-in user via GET, POST, PUT, DELETE requests
namespace CarTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarInfoController : ControllerBase
    {
        private readonly ICarInfoRepository _carInfoRepository;
        // Default Parameter Limits
        private int parameterMaxLength = 100;
        private int maxStockCount = 999999;
        private int minYear = 1855;
        private int currentYear = DateTime.Now.Year;

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
            if (userID == -1)
                return Unauthorized("Token is missing or invalid.");

            var cars = _carInfoRepository.GetCarsByUserId(userID);
            if (cars == null || !cars.Any()) return NotFound();
            return Ok(cars);
        }

        [HttpGet("make/{make}")]
        public IActionResult GetCarsByMake(string make)
        {
            // Input Validation
            if (make.Length > parameterMaxLength) // not accept if input exceeds defined max size
                return BadRequest("Invalid input for make - Input Length exceeded String Length!");
            if (!StringValidator.ContainsOnlyLegalCharacters(make)) // not accept any illegal characters
                return BadRequest("Invalid input for make - Input contains Illegal Characters!");

            var userId = GetUserIdFromToken();
            if (userId == -1)
                return Unauthorized("Token is missing or invalid.");

            var cars = _carInfoRepository.GetCarsByMake(userId, make);

            return Ok(cars);
        }

        [HttpGet("model/{model}")]
        public IActionResult GetCarsByModel(string model)
        {
            // Input Validation
            if (model.Length > parameterMaxLength) // not accept if input exceeds defined max size
                return BadRequest("Invalid input for model - Input Length exceeded String Length!");
            if (!StringValidator.ContainsOnlyLegalCharacters(model)) // not accept any illegal characters
                return BadRequest("Invalid input for model - Input contains Illegal Characters!");

            var userID = GetUserIdFromToken();
            if (userID == -1)
                return Unauthorized("Token is missing or invalid.");


            var cars = _carInfoRepository.GetCarsByModel(userID, model);
            return Ok(cars);
        }

        [HttpGet("make/{make}/model/{model}")]
        public IActionResult GetCarsByMakeAndModel(string make, string model)
        {
            // Input Validation
            if (make.Length > parameterMaxLength) // not accept if make exceeds defined max size
                return BadRequest("Invalid input for make - Input Length exceeded String Length!");
            if (!StringValidator.ContainsOnlyLegalCharacters(make)) // not accept any illegal characters
                return BadRequest("Invalid input for make - Input contains Illegal Characters!");
            if (model.Length > parameterMaxLength) // not accept if model exceeds defined max size
                return BadRequest("Invalid input for model - Input Length exceeded String Length!");
            if (!StringValidator.ContainsOnlyLegalCharacters(model)) // not accept any illegal characters
                return BadRequest("Invalid input for model - Input contains Illegal Characters!");

            var userID = GetUserIdFromToken();
            if (userID == -1)
                return Unauthorized("Token is missing or invalid.");

            var cars = _carInfoRepository.GetCarsByMakeAndModel(userID, make, model);
            return Ok(cars);
        }

        // Adds a new car entry for the logged-in user
        [HttpPost("add")]
        public IActionResult AddCar([FromBody] CarCreateDto carDto)
        {
            // ## Input Validation
            // Make is within 100 char limit, english words, doesn't contain restricted characters, not null
            // Model is within 100 char limit, english words, doesn't contain restricted characters, not null
            // Year is between 1855-<present> (first car made in 1855, probably will never have a car with that date but never know), not null
            // Stock level is non-negative and between 1-<stock limit>
            if (carDto.Make.Length > parameterMaxLength) // not accept if make exceeds defined max size
                return BadRequest("Invalid input for make - Input Length exceeded String Length!");
            if (!StringValidator.ContainsOnlyLegalCharacters(carDto.Make)) // not accept any illegal characters
                return BadRequest("Invalid input for make - Input contains Illegal Characters!");
            if (carDto.Model.Length > parameterMaxLength) // not accept if model exceeds defined max size
                return BadRequest("Invalid input for model - Input Length exceeded String Length!");
            if (!StringValidator.ContainsOnlyLegalCharacters(carDto.Model)) // not accept any illegal characters
                return BadRequest("Invalid input for model - Input contains Illegal Characters!");
            if (carDto.Year < minYear || carDto.Year > currentYear)
                return BadRequest($"Invalid input for year - Input must be between {minYear}-{currentYear}");
            if (carDto.StockLevel < 0 || carDto.StockLevel > maxStockCount) // must be between 0-<max stock count>
                return BadRequest($"Invalid input for stock level - Input is not between 0 and {maxStockCount}!");

            var userID = GetUserIdFromToken();
            if (userID == -1)
                return Unauthorized("Token is missing or invalid.");

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
            var userID = GetUserIdFromToken();
            if (userID == -1)
                return Unauthorized("Token is missing or invalid.");

            var success = _carInfoRepository.RemoveCar(carId, userID);
            if (!success) // fails if the carID isn't owned by the user or isn't present in the database.
                return NotFound($"Car with ID {carId} for the logged-in user not found.");
            return Ok($"Car with ID {carId} removed.");
        }

        // Updates the stock level for a specific car owned by the logged-in user. 
        [HttpPut("update-stock/{carId}")]
        public IActionResult UpdateStockLevel(int carId, [FromBody] CarInfoUpdateStockDto dto)
        {
            if (dto.StockLevel < 0 || dto.StockLevel > maxStockCount)
                return BadRequest($"Invalid input for stock level - Input is not between 0 and {maxStockCount}!");

            var userID = GetUserIdFromToken();
            if (userID == -1)
                return Unauthorized("Token is missing or invalid.");

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
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return -1; // -1 is invalid userID

            return userId;
        }


    }
}
