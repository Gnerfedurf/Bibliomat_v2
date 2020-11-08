using Bibliomat.Common.Model;
using Bibliomat.Repository.Abstract;
using Bibliomat.Repository.EntityFramework.Base;
using System.Linq;

namespace Bibliomat.Repository.EntityFramework
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        #region Public Methods

        /// <summary>
        /// Gets a User by Id.
        /// </summary>
        /// <param name="userId">The Id of the User in question.</param>
        /// <returns>A User object.</returns>
        public User GetUserById(int userId)
        {
            var query = DBContext.User.AsQueryable();

            query = query.Where(user => user.Id == userId);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Checks if a User exists.
        /// </summary>
        /// <param name="name">The name of the User in question.</param>
        /// <returns>True or false depending on if the User exists or not.</returns>
        public bool UserExists(string name)
        {
            var query = DBContext.User.AsQueryable();

            query = query.Where(user => user.Name == name);

            // Check if the User was found in the database
            if (query.Any())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Logs a User in.
        /// </summary>
        /// <param name="name">The name of the User in question.</param>
        /// <param name="pass">The password of the User in question.</param>
        /// <returns>The Id of the User or null if the login failed.</returns>
        public int? LogIn(string name, string pass)
        {
            var query = DBContext.User.AsQueryable();

            query = query.Where(user => user.Name == name);

            query = query.Where(user => user.Pass == pass);

            // Check if the User was found in the database
            if (query.Any())
            {
                // Get only one User object
                var user = query.FirstOrDefault();

                return user.Id;
            }

            return null;
        }

        /// <summary>
        /// Registeres a new User.
        /// </summary>
        /// <param name="name">The name of the User in question.</param>
        /// <param name="pass">The password of the User in question.</param>
        public void Register (string name, string pass)
        {
            // Create a new User with the name and password
            var user = new User()
            {
                Name = name,
                Pass = pass
            };

            DBContext.User.Add(user);
            DBContext.SaveChanges();
        }

        #endregion
    }
}
