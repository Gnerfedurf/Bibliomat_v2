using Bibliomat.Common.Model;
using System.Collections.Generic;

namespace Bibliomat.BusinessLogic.Abstract
{
    public interface IBookBusinessLogic
    {
        IList<Book> GetBooks();
        IList<Book> GetBooksByUserId(int userId);
        Book GetBook(int bookId);
        bool ReserveBook(int bookId, int userId);
        bool ReturnBook(int bookId);
    }
}
