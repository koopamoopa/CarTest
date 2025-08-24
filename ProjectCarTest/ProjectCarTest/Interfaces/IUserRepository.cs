using ProjectCarTest.Models;
using System.Collections.Generic;

namespace ProjectCarTest.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetAllUsers();
    }
}
