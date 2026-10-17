
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
                            .AsNoTracking()
                            .Include(u => u.UserPreferences)
                            .ToList();
        }

        public User? GetById(int id)
        {
            return _context.User
                .Include(u => u.UserPreferences)
                .FirstOrDefault(p => p.UserId == id);
        }

        public User Add(User entity)
        {
            _context.User.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public User? Update(int id, User entity)
        {
            var user = _context.User
                .Include(u => u.UserPreferences)
                .FirstOrDefault(p => p.UserId == id);

            if (user == null)
                return null;

            // 1. עדכון שדות הבסיס של המשתמש
            _context.Entry(user).CurrentValues.SetValues(entity);
            user.UserId = id;
            user.UpdatedAt = DateTime.UtcNow;

            // 2. טיפול בעדכון ההעדפות המקושרות (UserPreferences)
            if (entity.UserPreferences != null)
            {
                _context.UserPreference.RemoveRange(user.UserPreferences);
                user.UserPreferences = entity.UserPreferences;
            }

            _context.SaveChanges();
            return user;
        }

      
    }
}
