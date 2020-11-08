using Bibliomat.BusinessLogic.Abstract;
using Bibliomat.Common.Model;
using Bibliomat.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bibliomat.WebApp.Controllers
{
    public class HomeController : Controller
    {
        #region Instance Variables

        private readonly IBookBusinessLogic _bookBusinessLogic;

        #endregion

        #region Constructor

        public HomeController(IBookBusinessLogic bookBusinessLogic)
        {
            _bookBusinessLogic = bookBusinessLogic;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Prepares and passes the needed data for the home-page.
        /// </summary>
        /// <returns>The home-view.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View(prepareHomeViewModel());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Prepares the HomeViewModel.
        /// </summary>
        /// <returns>The prepared HomeViewModel.</returns>
        private HomeViewModel prepareHomeViewModel()
        {
            // Get all Books
            var books = _bookBusinessLogic.GetBooks();

            // Create a List of BookViewModels
            var booksForViewModel = new List<BookViewModel>();

            // Fill the List with the gotten Books
            foreach (var book in books)
            {
                var bookForViewModel = new BookViewModel()
                {
                    Id     = book.Id,
                    Title  = book.Title,
                    UserId = book.UserId
                };

                booksForViewModel.Add(bookForViewModel);
            }

            // Get error- and success-messages if there are any
            var successMessage = TempData["SuccessMessage"];
            var errorMessage = TempData["ErrorMessage"];

            // Create a new CollectionViewModel with all previously set values
            var homeViewModel = new HomeViewModel() {
                Books = booksForViewModel,
                SuccessMessage = (string)successMessage,
                ErrorMessage = (string)errorMessage
            };

            return homeViewModel;
        }

        #endregion
    }
}
