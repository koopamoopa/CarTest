namespace ProjectCarTest.Dto
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string CompanyName { get; set; }  // if you would like to mention the company name for frontend.
        public string Result { get; set; }
    }
}
