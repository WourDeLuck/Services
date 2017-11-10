using Services.Common;

namespace Services.DataAccess
{
    /// <summary>
    /// Repository interface with standart methods
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="user">User info</param>
        void Create(User user);

        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>User</returns>
        User Get(int id);

        /// <summary>
        /// Updates user's info
        /// </summary>
        /// <param name="user">User info</param>
        void Update(User user);

        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <param name="id">User's id</param>
        void Delete(int id);

        // Additional methods
        /// <summary>
        /// Gets user by login
        /// </summary>
        /// <param name="login">User's login</param>
        /// <returns>User</returns>
        User Get(string login);

        /// <summary>
        /// Gets user by email
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>User</returns>
        User GetByEmail(string email);
    }
}