using Bibliomat.BusinessLogic.Abstract;
using Bibliomat.Common.Model;
using Bibliomat.Repository.Abstract;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Bibliomat.BusinessLogic.Services
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        #region Instance Variables

        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructor

        public UserBusinessLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a User by Id.
        /// </summary>
        /// <param name="userId">The Id of the User in question.</param>
        /// <returns>A User object.</returns>
        public User GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }

        /// <summary>
        /// Checks if a User exists.
        /// </summary>
        /// <param name="name">The name of the User in question.</param>
        /// <returns>True or false depending on if the User exists or not.</returns>
        public bool UserExists(string name)
        {
            return _userRepository.UserExists(name);
        }

        /// <summary>
        /// Logs a User in.
        /// </summary>
        /// <param name="name">The name of the User in question.</param>
        /// <param name="pass">The password of the User in question.</param>
        /// <returns>The Id of the User or null if the login failed.</returns>
        public int? LogIn(string name, string pass)
        {
            return _userRepository.LogIn(name, Encode(pass));
        }

        /// <summary>
        /// Registeres a new User.
        /// </summary>
        /// <param name="name">The name of the User in question.</param>
        /// <param name="pass">The password of the User in question.</param>
        /// <returns>True or false depending on if the User could be registered or not.</returns>
        public bool Register(string name, string pass)
        {
            // Check if the User does not exist already
            if (!UserExists(name))
            {
                // Encode the password
                var encodedPass = Encode(pass);

                // Pass the name and newly encoded password to the repository
                _userRepository.Register(name, encodedPass);

                return true;
            }

            return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Encodes a Password. This is a one-way encodation that cannot be easily reversed.
        /// </summary>
        /// <param name="pass">The password to be encoded.</param>
        /// <returns>The newly encoded password as a string.</returns>
        private string Encode(string pass)
        {
            // Encode the password using MD5
            byte[] bytes = Encoding.Default.GetBytes(pass);
            byte[] encodedBytes = new MD5CryptoServiceProvider().ComputeHash(bytes);

            return BitConverter.ToString(encodedBytes);
        }

        #endregion
    }
}
