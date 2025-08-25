using System.ComponentModel.DataAnnotations;

namespace ProjectCarTest.Dto
{
    public class CarInfoUpdateStockDto
    {
        [Required]
        public int StockLevel { get; set; }
    }
}
