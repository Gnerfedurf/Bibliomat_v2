using Bibliomat.WebApp.Models.Shared;
using System.Collections.Generic;

namespace Bibliomat.WebApp.Models
{
    public class CollectionViewModel : BaseViewModel
    {
        public IList<BookViewModel> Books { get; set; }
    }
}
