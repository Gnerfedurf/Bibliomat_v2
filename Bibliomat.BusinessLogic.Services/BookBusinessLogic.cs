using Bibliomat.BusinessLogic.Abstract;
using Bibliomat.Common.Model;
using Bibliomat.Repository.Abstract;
using System.Collections.Generic;

namespace Bibliomat.BusinessLogic.Services
{
    public class BookBusinessLogic : IBookBusinessLogic
    {
        #region Instance Variables

        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructor

        public BookBusinessLogic(
            IBookRepository bookRepository,
            IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all Books.
        /// </summary>
        /// <returns>A list containing all Book objects.</returns>
        public IList<Book> GetBooks()
        {
            return _bookRepository.GetBooks();
        }

        /// <summary>
        /// Gets all Books a User owns.
        /// </summary>
        /// <param name="userId">The Id of the User in question.</param>
        /// <returns>A list containing all Book objects of a certain User.</returns>
        public IList<Book> GetBooksByUserId(int userId)
        {
            return _bookRepository.GetBooksByUserId(userId);
        }

        /// <summary>
        /// Gets a Book by its Id.
        /// </summary>
        /// <param name="bookId">The Id of the Book in question.</param>
        /// <returns>A Book object.</returns>
        public Book GetBook(int bookId)
        {
            return _bookRepository.GetBook(bookId);
        }

        /// <summary>
        /// Reserves a Book in the name of a User.
        /// </summary>
        /// <param name="bookId">The Id of the Book to be reserved.</param>
        /// <param name="userId">The Id of the User who reserves the Book.</param>
        /// <returns>True or false depending on if the reservation was successful or not.</returns>
        public bool ReserveBook(int bookId, int userId)
        {
            // Get the Book and User object
            var book = _bookRepository.GetBook(bookId);
            var user = _userRepository.GetUserById(userId);

            // Check if the Book and the User exist
            if (book != null && _userRepository.UserExists(user.Name))
            {
                _bookRepository.ReserveBook(bookId, userId);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a Book.
        /// </summary>
        /// <param name="bookId">The Id of the Book to be returned.</param>
        /// <returns>True or false depending on if the Book was retourned successfully or not.</returns>
        public bool ReturnBook(int bookId)
        {
            // Get the Book object
            var book = _bookRepository.GetBook(bookId);

            // Check if the Book exists
            if (book != null)
            {
                _bookRepository.ReturnBook(bookId);

                return true;
            }

            return false;
        }

        #endregion
    }
}
