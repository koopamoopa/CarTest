namespace ProjectCarTest.Dto
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;  // if you would like to mention the company name for frontend.
        public string Result { get; set; } = string.Empty;
    }
}
