using ProjectCarTest.Data;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectCarTest.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public User? GetUserByUsernameAndPassword(string username, string password)
        {
            return _context.Users
                .FirstOrDefault(u => u.username == username && u.password == password);
        }
    }
}
