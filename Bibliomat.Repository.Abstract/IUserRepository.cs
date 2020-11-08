using Bibliomat.Common.Model;

namespace Bibliomat.Repository.Abstract
{
    public interface IUserRepository
    {
        bool UserExists(string name);
        User GetUserById(int userId);
        int? LogIn(string name, string pass);
        void Register(string name, string pass);
    }
}
