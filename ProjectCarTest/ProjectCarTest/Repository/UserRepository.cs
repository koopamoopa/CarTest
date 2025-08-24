using ProjectCarTest.Data;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCarTest.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users
                .OrderBy(u => u.userID)
                .ToList();
        }
    }
}
