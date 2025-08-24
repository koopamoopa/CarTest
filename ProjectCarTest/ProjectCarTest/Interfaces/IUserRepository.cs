using ProjectCarTest.Dto;
using ProjectCarTest.Models;
using System.Collections.Generic;

namespace ProjectCarTest.Interfaces
{
    public interface IUserRepository
    {
        //User? GetUserByUsernameAndPassword(string username, string password);
        LoginResponseDto Login(LoginRequestDto request);

    }
}
