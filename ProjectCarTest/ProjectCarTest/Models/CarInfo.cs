using ProjectCarTest.Models;

namespace ProjectCarTest.Models
{
    public class CarInfo
    {
        public int carID { get; set; }

        public User? User { get; set; } 
        public int userID { get; set; } // foreign key

        public int stockLevel { get; set; }
        public int year { get; set; }
        public string make { get; set; } = string.Empty;
        public string model { get; set; } = string.Empty;


    }
}
