using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Services.DataAccess;
using System.Text.RegularExpressions;

namespace Services
{
    /// <summary>
    /// Implementation of user service interface
    /// </summary>
    public class UserService : IUserService
    {
        // Repository initialization
        private readonly IUserRepository _repository;

        // Constructor usage
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">User and user's info to use for registration.</param>
        public void Register(User user)
        {
            // Validation
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(user.Login))
            {
                throw new ValidationException($"Incorrect login {user.Login}");
            }

            if (user.Password.Length < 3)
            {
                throw new ValidationException($"Incorrect password {user.Password}");
            }

            _repository.Create(user);
        }

        /// <summary>
        /// Authorizes a user.
        /// </summary>
        /// <param name="login">Login to authorize a user with.</param>
        /// <param name="password">Password that protects the user.</param>
        /// <returns></returns>
        public bool Login(string login, string password)
        {
            // Validation
            if (string.IsNullOrEmpty(login))
            {
                throw new ValidationException($"Incorrect login {login}");
            }

            else if (string.IsNullOrEmpty(password))
            {
                throw new ValidationException($"Incorrect password {password}");
            }
            var user = _repository.Get(login);

            if (user != null && user.Password == password)
                return true;
            else return false;           
        }

        /// <summary>
        /// Returns a user by their id.
        /// </summary>
        /// <param name="id">Id to operate with.</param>
        /// <returns></returns>
        public User GetUser(int id)
        {       
            return _repository.Get(id);
        }

        /// <summary>
        /// Returns a user by their login.
        /// </summary>
        /// <param name="login">Login to return user by.</param>
        /// <returns></returns>
        public User GetUserByLogin(string login)
        {
            // Validation
            if (string.IsNullOrEmpty(login))
            {
                throw new ValidationException($"Incorrect login {login}");
            }
            return _repository.Get(login);           
        }

        /// <summary>
        /// Delete current user.
        /// </summary>
        /// <param name="id">Id to delete user by.</param>
        public void Unregister(int id)
        {
            _repository.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool CheckUserByEmail(string email)
        {
            bool checkedEmail = false;

            if(IsEmail(email))
            {
                var usr = _repository.GetByEmail(email);
                checkedEmail = usr == null ? false : true;
            }

            return checkedEmail;
        }

        /// <summary>
        /// Email validation using regular expressions.
        /// </summary>
        /// <param name="email">Email to operate with.</param>
        /// <returns></returns>
        public bool IsEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }
    }
}
