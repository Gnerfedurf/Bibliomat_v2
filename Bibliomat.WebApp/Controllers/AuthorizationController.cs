using Bibliomat.BusinessLogic.Abstract;
using Bibliomat.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bibliomat.WebApp.Controllers
{
    public class AuthorizationController : Controller
    {
        #region Instance Varaibles

        private readonly IUserBusinessLogic _userBusinessLogic;

        #endregion

        #region Constructor

        public AuthorizationController(IUserBusinessLogic userBusinessLogic)
        {
            _userBusinessLogic = userBusinessLogic;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Prepares and passes the needed data for the login-page.
        /// </summary>
        /// <param name="logInViewModel">The ViewModel for the login-page.</param>
        /// <returns>The login-view.</returns>
        [HttpGet]
        public IActionResult Index(LogInViewModel logInViewModel)
        {
            return View(prepareLogInViewModel(logInViewModel));
        }

        /// <summary>
        /// Prepares and passes the needed data for the registration-page.
        /// </summary>
        /// <param name="registerViewModel">The ViewModel for the registration-page.</param>
        /// <returns>The registration-view.</returns>
        [HttpGet]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            return View(prepareRegisterViewModel(registerViewModel));
        }

        /// <summary>
        /// Logs the User in.
        /// </summary>
        /// <param name="logInViewModel">The ViewModel for the login-page.</param>
        /// <returns>The Homepage-view if the login was successful or the login-view with an Errormessage if not.</returns>
        [HttpPost]
        public IActionResult DoLogIn(LogInViewModel logInViewModel)
        {
            // Get the Id of the User
            var userId =_userBusinessLogic.LogIn(logInViewModel.Name, logInViewModel.Pass);

            // Check if the login failed
            if (userId == null)
            {
                return View("Index", new LogInViewModel()
                {
                    ErrorMessage = "Incorrect username or password"
                });
            }

            // Create new Session
            HttpContext.Session.SetInt32("UserId", (int)userId);

            // Pass the success-message through the TempData, because the model cannot be passed
            TempData["SuccessMessage"] = "Successfully logged in";

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Registers the User.
        /// </summary>
        /// <param name="registerViewModel">The ViewModel for the registration-page.</param>
        /// <returns>The Homepage-view if the registration was successful or the registration-view with an Errormessage if not.</returns>
        [HttpPost]
        public IActionResult DoRegister(RegisterViewModel registerViewModel)
        {
            // Declare some easier to use variables
            var name = registerViewModel.Name;
            var pass = registerViewModel.Pass;

            // Try to register the User
            if (_userBusinessLogic.Register(name, pass))
            {
                // Log in
                var userId = _userBusinessLogic.LogIn(name, pass);

                // Create new Session
                HttpContext.Session.SetInt32("UserId", (int)userId);

                // Pass the success-message through the TempData, because the model cannot be passed
                TempData["SuccessMessage"] = "Successfully registered and logged in";

                return RedirectToAction("Index", "Home");
            }

            return View("Register", new RegisterViewModel()
            {
                ErrorMessage = "Username is already taken"
            });
        }

        /// <summary>
        /// Logs the User out.
        /// </summary>
        /// <returns>The Homepage-view.</returns>
        [HttpGet]
        public IActionResult LogOut()
        {
            // Destroy the Session
            HttpContext.Session.Clear();

            // Pass the success-message through the TempData, because the model cannot be passed
            TempData["SuccessMessage"] = "Successfully logged out";

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Prepares the LoginViewModel.
        /// </summary>
        /// <param name="logInViewModel">The ViewModel for the login-page.</param>
        /// <returns>The prepared LoginViewModel.</returns>
        private LogInViewModel prepareLogInViewModel(LogInViewModel logInViewModel)
        {
            // Create a new LoginViewModel if it does not exist already
            if (logInViewModel == null)
            {
                return new LogInViewModel();
            }

            return logInViewModel;
        }

        /// <summary>
        /// Prepares the RegisterViewModel.
        /// </summary>
        /// <param name="registerViewModel">The ViewModel for the registration-page.</param>
        /// <returns>The prepared RegisterViewModel.</returns>
        private RegisterViewModel prepareRegisterViewModel(RegisterViewModel registerViewModel)
        {
            // Create a new RegisterViewModel if it does not exist already
            if (registerViewModel == null)
            {
                return new RegisterViewModel();
            }

            return registerViewModel;
        }

        #endregion
    }
}
