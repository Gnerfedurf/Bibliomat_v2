using Bibliomat.BusinessLogic.Abstract;
using Bibliomat.Common.Model;
using Bibliomat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bibliomat.WebApp.Controllers
{
    public class CollectionController : Controller
    {
        #region Instance Variables

        private readonly IBookBusinessLogic _bookBusinessLogic;

        #endregion

        #region Constructor

        public CollectionController(IBookBusinessLogic bookBusinessLogic)
        {
            _bookBusinessLogic = bookBusinessLogic;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Prepares and passes the needed data for the your-books-page.
        /// </summary>
        /// <returns>The collection-view.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View(prepareCollectionViewModel());
        }

        /// <summary>
        /// Returns a Book.
        /// </summary>
        /// <param name="bookId">The Id of the Book to be retourned.</param>
        /// <returns>The collection-view.</returns>
        [HttpGet]
        public IActionResult Return([FromQuery] int bookId)
        {
            // Check if the User is logged in
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // Pass the error-message through the TempData, because the model cannot be passed
                TempData["ErrorMessage"] = $"It seems you are not logged in";

                return RedirectToAction("Index", "Home");
            }

            // Get the Book from the passed bookId
            var book = _bookBusinessLogic.GetBook(bookId);

            // Check if the Book exists
            if (book != null)
            {
                // Check if the currently logged in User is the owner of the book
                if (book.UserId == HttpContext.Session.GetInt32("UserId"))
                {
                    // Try to return the book
                    var returnSuccessful = _bookBusinessLogic.ReturnBook(bookId);

                    // Check if the book has been returned successfully
                    if (returnSuccessful)
                    {
                        // Pass the success-message through the TempData, because the model cannot be passed
                        TempData["Successmessage"] = $"The book has been returned";

                        return View("Index", prepareCollectionViewModel());
                    }

                    // Pass the error-message through the TempData, because the model cannot be passed
                    TempData["ErrorMessage"] = $"The book \"{book.Title}\" could not be returned";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Pass the error-message through the TempData, because the model cannot be passed
                    TempData["ErrorMessage"] = $"You do not own the book \"{book.Title}\"";

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // Pass the error-message through the TempData, because the model cannot be passed
                TempData["ErrorMessage"] = $"The book with id {bookId} does not exist";

                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region Privare Methods

        /// <summary>
        /// Prepares the CollectionViewModel.
        /// </summary>
        /// <returns>The prepared CollectionViewModel.</returns>
        private CollectionViewModel prepareCollectionViewModel()
        {
            // Get all Books of the currently logged in User
            var books = _bookBusinessLogic.GetBooksByUserId((int)HttpContext.Session.GetInt32("UserId"));

            // Create a List of BookViewModels
            var booksForViewModel = new List<BookViewModel>();

            // Fill the List with the gotten Books
            foreach (var book in books)
            {
                var bookForViewModel = new BookViewModel()
                {
                    Id = book.Id,
                    Title = book.Title,
                    UserId = book.UserId
                };

                booksForViewModel.Add(bookForViewModel);
            }

            // Get error- and success-messages if there are any
            var successMessage = TempData["SuccessMessage"];
            var errorMessage = TempData["ErrorMessage"];

            // Create a new CollectionViewModel with all previously set values
            return new CollectionViewModel() {
                Books = booksForViewModel,
                SuccessMessage = (string)successMessage,
                ErrorMessage = (string)errorMessage
            };
        }

        #endregion
    }
}

