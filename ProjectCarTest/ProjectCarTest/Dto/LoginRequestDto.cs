namespace ProjectCarTest.Dto
{
    public class LoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }  // plain text here, will be hashed/validated in service
    }

}
