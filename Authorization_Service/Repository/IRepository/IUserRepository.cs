using Authorization_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization_Service.Repository.IRepository
{
    public interface IUserRepository
    {
        bool isUserUnique(string useraname);

        UserModel Authenticate(string username, string password);

        UserModel Register(string username, string password);

    }
}
