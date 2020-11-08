using Bibliomat.BusinessLogic.Abstract;
using Bibliomat.Common.Model;
using Bibliomat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bibliomat.WebApp.Controllers
{
    public class DetailedController : Controller
    {
        #region Instance Variables

        private readonly IBookBusinessLogic _bookBusinessLogic;
        private readonly IUserBusinessLogic _userBusinessLogic;

        #endregion

        #region MyRegion

        public DetailedController(
            IBookBusinessLogic bookBusinessLogic,
            IUserBusinessLogic userBusinessLogic)
        {
            _bookBusinessLogic = bookBusinessLogic;
            _userBusinessLogic = userBusinessLogic;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Shows the Detailed View of a Book.
        /// </summary>
        /// <param name="bookId">The Id of the Book to be shown.</param>
        /// <returns>The detailed-view.</returns>
        [HttpGet]
        public IActionResult Index([FromQuery, Required] int bookId)
        {
            // Get the Book from the passed bookId
            var book = _bookBusinessLogic.GetBook(bookId);

            // Prepare the model
            var model = prepareDetailedViewModel(book);

            return View(model);
        }

        /// <summary>
        /// Returns a Book.
        /// </summary>
        /// <param name="bookId">The Id of the Book to be retourned.</param>
        /// <returns>The detailed-view.</returns>
        [HttpGet]
        public IActionResult Return([FromQuery, Required] int bookId)
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

                        // Prepare the model
                        var model = prepareDetailedViewModel(book);

                        return View("Index", model);
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

        /// <summary>
        /// Reserves a Book.
        /// </summary>
        /// <param name="bookId">The Id of the Book to be reserved.</param>
        /// <param name="userId">The Id of the User who wants to reserve the Book.</param>
        /// <returns>The detailed-view.</returns>
        [HttpGet]
        public IActionResult Reserve([FromQuery, Required] int bookId)
        {
            // Check if the User is logged in
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // Pass the error-message through the TempData, because the model cannot be passed
                TempData["ErrorMessage"] = $"It seems you are not logged in";

                return RedirectToAction("Index", "Home");
            }

            // Get the User by the Id from the Session and the Book by the bookId
            var user = _userBusinessLogic.GetUserById((int)HttpContext.Session.GetInt32("UserId"));
            var book = _bookBusinessLogic.GetBook(bookId);

            // Check if the Book exists
            if (book != null)
            {
                // Check if the User exists
                if (_userBusinessLogic.UserExists(user?.Name))
                {
                    // Try to reserve the Book
                    var reserveSuccessful = _bookBusinessLogic.ReserveBook(bookId, user.Id);

                    // Check if the Book has been reserved successfully
                    if (reserveSuccessful)
                    {
                        // Pass the success-message through the TempData, because the model cannot be passed
                        TempData["Successmessage"] = $"The book has been reserved";

                        // Prepare the model
                        var model = prepareDetailedViewModel(book);

                        return View("Index", model);
                    }

                    // Pass the error-message through the TempData, because the model cannot be passed
                    TempData["ErrorMessage"] = $"The book \"{book.Title}\" could not be reserved";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Pass the error-message through the TempData, because the model cannot be passed
                    TempData["ErrorMessage"] = $"There has been an unexpected Error with your session";

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

        #region Private Methods

        /// <summary>
        /// Prepares the DetailedViewModel.
        /// </summary>
        /// <param name="book">The Book object to be shown.</param>
        /// <returns>The prepared DetailedViewModel.</returns>
        private DetailedViewModel prepareDetailedViewModel(Book book)
        {
            // Prepare a BookViewModel with the same values as the passed Book
            var bookForViewModel = new BookViewModel()
            {
                Id          = book.Id,
                Title       = book.Title,
                Description = book.Description,
                UserId      = book.UserId
            };

            // Get error- and success-messages if there are any
            var successMessage = TempData["SuccessMessage"];
            var errorMessage = TempData["ErrorMessage"];

            // Create a new CollectionViewModel with all previously set values
            return new DetailedViewModel() {
                Book = bookForViewModel,
                SuccessMessage = (string)successMessage,
                ErrorMessage = (string)errorMessage
            };
        }

        #endregion
    }
}
