using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data;
using WorkFM.Common.Data.Users;
using WorkFM.Common.Dto;
using WorkFM.Common.Models.Users;

namespace WorkFM.BL.Services.Auth
{
    public interface IAuthBL
    {
        public Task<ServiceResponse> Login(UserLogin userLogin);
        public Task<ServiceResponse> Logout();
        public Task<ServiceResponse> Register(UserRegister userRegister);
        //public Task<ServiceResponse> RenewToken(TokenModel tokenModel);
    }
}
