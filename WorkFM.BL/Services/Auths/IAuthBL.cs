using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data;
using WorkFM.Common.Models;
using WorkFM.Common.Models.Users;

namespace WorkFM.BL.Services.Auth
{
    public interface IAuthBL
    {
        public Task<ServiceResponse> Login(string username, string password);
        public Task<ServiceResponse> Logout();
        public Task<ServiceResponse> Register(UserDto userRegister);
        public Task<ServiceResponse> RenewToken(TokenModel tokenModel)
    }
}
