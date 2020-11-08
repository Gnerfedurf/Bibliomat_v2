using Bibliomat.Common.Model;
using Bibliomat.Repository.Abstract;
using Bibliomat.Repository.EntityFramework.Base;
using System.Collections.Generic;
using System.Linq;

namespace Bibliomat.Repository.EntityFramework
{
    public class BookRepository : BaseRepository, IBookRepository
    {
        #region Public Methods

        /// <summary>
        /// Gets all Books.
        /// </summary>
        /// <returns>A list containing all Book objects.</returns>
        public IList<Book> GetBooks()
        {
            var query = DBContext.Book.AsQueryable();

            query = query.OrderBy(book => book.Title);

            return query.ToList();
        }

        /// <summary>
        /// Gets all Books a User owns.
        /// </summary>
        /// <param name="userId">The Id of the User in question.</param>
        /// <returns>A list containing all Book objects of a certain User.</returns>
        public IList<Book> GetBooksByUserId(int userId)
        {
            var query = DBContext.Book.AsQueryable();

            query = query.Where(book => book.UserId == userId);

            query = query.OrderBy(book => book.Title);

            return query.ToList();
        }

        /// <summary>
        /// Gets a Book by its Id.
        /// </summary>
        /// <param name="bookId">The Id of the Book in question.</param>
        public Book GetBook(int bookId)
        {
            var query = DBContext.Book.AsQueryable();

            query = query.Where(book => book.Id == bookId);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Reserves a Book in the name of a User.
        /// </summary>
        /// <param name="bookId">The Id of the Book to be reserved.</param>
        /// <param name="userId">The Id of the User who reserves the Book.</param>
        public void ReserveBook(int bookId, int userId)
        {
            var book = GetBook(bookId);

            book.UserId = userId;

            DBContext.SaveChanges();
        }

        /// <summary>
        /// Returns a Book.
        /// </summary>
        /// <param name="bookId">The Id of the Book to be returned.</param>
        public void ReturnBook(int bookId)
        {
            var book = GetBook(bookId);

            book.UserId = null;

            DBContext.SaveChanges();
        }

        #endregion
    }
}
