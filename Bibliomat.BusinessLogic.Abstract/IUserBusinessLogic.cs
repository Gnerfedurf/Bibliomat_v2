using Bibliomat.Common.Model;

namespace Bibliomat.BusinessLogic.Abstract
{
    public interface IUserBusinessLogic
    {
        bool UserExists(string name);
        User GetUserById(int userId);
        int? LogIn(string name, string pass);
        bool Register(string name, string pass);
    }
}
