namespace ProjectCarTest.Dto
{
    public class CarCreateDto
    {
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public int StockLevel { get; set; }
    }
}
