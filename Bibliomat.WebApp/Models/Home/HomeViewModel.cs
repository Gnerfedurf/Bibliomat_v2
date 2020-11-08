using Bibliomat.WebApp.Models.Shared;
using System.Collections.Generic;

namespace Bibliomat.WebApp.Models
{
    public class HomeViewModel : BaseViewModel
    {
        public IList<BookViewModel> Books { get; set; }
    }
}
