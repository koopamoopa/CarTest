using ProjectCarTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectCarTest.Data
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            _context.Database.EnsureCreated();

            if (_context.Users.Any())
                return; // DB already seeded

            // First user and car
            var user1 = new User
            {
                username = "audi_owner",
                password = "securepass",
                companyName = "Audi Lovers Inc."
            };
            _context.Users.Add(user1);
            _context.SaveChanges();

            var car1 = new CarInfo
            {
                User = user1,
                stockLevel = 3,
                year = 2018,
                make = "Audi",
                model = "A4"
            };
            _context.CarInfos.Add(car1);
            _context.SaveChanges();

            // Second user and car
            var user2 = new User
            {
                username = "bmw_owner",
                password = "anotherpass",
                companyName = "BMW Enthusiasts Ltd."
            };
            _context.Users.Add(user2);
            _context.SaveChanges();

            var car2 = new CarInfo
            {
                User = user2,
                stockLevel = 5,
                year = 2020,
                make = "BMW",
                model = "X5"
            };
            _context.CarInfos.Add(car2);
            _context.SaveChanges();

            // Third user and car (example)
            var user3 = new User
            {
                username = "merc_owner",
                password = "pass1234",
                companyName = "Mercedes Fans Co."
            };
            _context.Users.Add(user3);
            _context.SaveChanges();

            var car3 = new CarInfo
            {
                User = user3,
                stockLevel = 2,
                year = 2021,
                make = "Mercedes",
                model = "C-Class"
            };
            _context.CarInfos.Add(car3);
            _context.SaveChanges();

            // Additional cars for user1
            var car4 = new CarInfo
            {
                User = user1,
                stockLevel = 4,
                year = 2019,
                make = "Audi",
                model = "Q5"
            };
            _context.CarInfos.Add(car4);

            var car5 = new CarInfo
            {
                User = user1,
                stockLevel = 2,
                year = 2020,
                make = "Audi",
                model = "A6"
            };
            _context.CarInfos.Add(car5);

            _context.SaveChanges();


            // Add more users/cars as needed
        }
    }
}
