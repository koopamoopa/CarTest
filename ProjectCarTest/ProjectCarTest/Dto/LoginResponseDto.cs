namespace ProjectCarTest.Dto
{
    public class LoginResponseDto
    {
        //public string Token { get; set; } // future use for JWT or session token
        public string Username { get; set; }
        public string CompanyName { get; set; }  // if you would like to mention the company name for frontend.
    }
}
