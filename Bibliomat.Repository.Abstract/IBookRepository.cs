using Bibliomat.Common.Model;
using System.Collections.Generic;

namespace Bibliomat.Repository.Abstract
{
    public interface IBookRepository
    {
        IList<Book> GetBooks();
        IList<Book> GetBooksByUserId(int userId);
        Book GetBook(int bookId);
        void ReserveBook(int bookId, int userId);
        void ReturnBook(int bookId);
    }
}
