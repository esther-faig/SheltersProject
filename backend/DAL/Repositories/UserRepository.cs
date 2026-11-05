
using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly DB_Shelters _context;

        public UserRepository(
            DB_Shelters context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.User
                            .Include(u => u.UserPreferences)
                                .ThenInclude(up => up.Preference)
                            .Include(u => u.Shelters)
                            .AsNoTracking()
                            .ToList();
        }

        public User? GetById(int id)
        {
            return _context.User
                 .Include(u => u.UserPreferences)
                     .ThenInclude(up => up.Preference)
                 .Include(u => u.Shelters)
                 .FirstOrDefault(u => u.UserId == id);
        }
        public User? GetByEmail(string email)
        {
            return _context.User
                .FirstOrDefault(u => u.Email == email);
        }
        public bool EmailExists(string email) =>
        _context.User.Any(u => u.Email == email);
        public User Add(User entity)
        {
            _context.User.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public User? Update(User entity)
        {
            _context.User.Update(entity);
            _context.SaveChanges();
            return GetById(entity.UserId)!;
        }
      
    }
}
