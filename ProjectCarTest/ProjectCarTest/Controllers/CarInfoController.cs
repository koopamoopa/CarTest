//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;

namespace CarTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarInfoController : Controller
    {
        // bring in repository
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(carInfos);
        }

        [HttpGet("user/{userID}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CarInfo>))]
        [ProducesResponseType(404)]
        public IActionResult GetCarsByUserId(int userID)
        {
            var cars = _carInfoRepository.GetCarsByUserId(userID);

            if (cars == null || !cars.Any())
                return NotFound();

            return Ok(cars);
        }

        [HttpGet("user/{userId}/make/{make}")]
        public IActionResult GetCarsByMake(int userId, string make)
        {
            var cars = _carInfoRepository.GetCarsByMake(userId, make);
            return Ok(cars);
        }

        [HttpGet("user/{userId}/model/{model}")]
        public IActionResult GetCarsByModel(int userId, string model)
        {
            var cars = _carInfoRepository.GetCarsByModel(userId, model);
            return Ok(cars);
        }

        [HttpGet("user/{userId}/make/{make}/model/{model}")]
        public IActionResult GetCarsByMakeAndModel(int userId, string make, string model)
        {
            var cars = _carInfoRepository.GetCarsByMakeAndModel(userId, make, model);
            return Ok(cars);
        }



    }
}
