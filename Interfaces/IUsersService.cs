using System.Collections.Generic;
using tasks.Models;

namespace tasks.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();

        User GetById(int id);

        int Add(User newUser);

        bool Update(User newUser);

        bool Delete(int id);

        User getUser(string name, string password);
    }
}
