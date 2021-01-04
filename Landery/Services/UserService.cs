using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Landery.Infrastructure;
using Landery.Models;
using System.Linq;

namespace Landery.Services
{
    public interface IUserService
    {
        bool IsAnExistingUser(string Email);
        bool IsValidUserCredentials(string Email, string password);
        User getUserByEmail(string email);
        string GetUserRole(string Email);
        void AddUser(string firstName, string lastName, string email, string password);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly Context _context = new Context();

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public bool IsValidUserCredentials(string Email, string password)
        {
            _logger.LogInformation($"Validating user [{Email}]");
            if (string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            User user = _context.Users.FirstOrDefault(u => u.Email == Email);

            if (user == default(User))
            {
                return false;
            }
            else
            {
                return user.Password == password;
            }
        }

        public bool IsAnExistingUser(string Email)
        {
            if(_context.Users.FirstOrDefault(u => u.Email == Email) == default(User))
            {
                return false;
            }

            return true;
        }

        public User getUserByEmail(string email)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }


        public void AddUser(string firstName, string lastName, string email, string password)
        {
            User user = new User();
            user.Email = email;
            user.Password = password;
            user.FirstName = firstName;
            user.LastName = lastName;
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public string GetUserRole(string Email)
        {
            if (!IsAnExistingUser(Email))
            {
                return string.Empty;
            }

            return UserRoles.BasicUser;
        }
    }

    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BasicUser = nameof(BasicUser);
    }
}
