using System.ComponentModel.DataAnnotations;

namespace ProjectCarTest.Dto
{
    public class LoginRequestDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        //[Contain "eng alphabet, !@#$%, numbers"]
        public string Password { get; set; } 
    }

}
