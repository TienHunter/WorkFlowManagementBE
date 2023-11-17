using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Users;
using WorkFM.Common.Dto;
using WorkFM.Common.Models.Users;

namespace WorkFM.BL.Services.Users
{
    public interface IUserBL:IBaseBL<UserDto, User>
    {
        /// <summary>
        /// register user
        /// </summary>
        /// <param name="userRegister"></param>
        /// <returns></returns>
        public Task<ServiceResponse> CreateUser(UserRegister userRegister);
    }
}
