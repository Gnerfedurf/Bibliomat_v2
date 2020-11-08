using Bibliomat.Common.Model;

namespace Bibliomat.Repository.EntityFramework.Base
{
    public class BaseRepository
    {
        protected BibliomatContext DBContext;

        public BaseRepository()
        {
            this.DBContext = new BibliomatContext();
        }
    }
}
