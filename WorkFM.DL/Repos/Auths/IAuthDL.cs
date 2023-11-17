using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Models.Users;

namespace WorkFM.DL.Repos.Auths
{
    public interface IAuthDL
    {
        public Task<User> Login(string username);

        public Task Logout();

        public Task Register();
    }
}
