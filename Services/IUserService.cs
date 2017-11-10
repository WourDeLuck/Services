using Services.Common;

namespace Services
{
    /// <summary>
    /// User service interface
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates a user in database.
        /// </summary>
        /// <param name="user">User info</param>
        void Register(User user);

        /// <summary>
        /// Autorized a user.
        /// </summary>
        /// <param name="login">User's login</param>
        /// <param name="password">User's password</param>
        /// <returns></returns>
        bool Login(string login, string password);

        /// <summary>
        /// Returns a user by id
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns></returns>
        User GetUser(int id);

        /// <summary>
        /// Returns a user by login
        /// </summary>
        /// <param name="login">User's login</param>
        /// <returns></returns>
        User GetUserByLogin(string login);

        /// <summary>
        /// Deletes a user by id
        /// </summary>
        /// <param name="id">User's id</param>
        void Unregister(int id);

        /// <summary>
        /// Checks a user by their email
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns></returns>
        bool CheckUserByEmail(string email);
    }
}