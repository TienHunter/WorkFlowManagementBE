using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Users;
using WorkFM.Common.Models.Users;

namespace WorkFM.BL.Services.Users
{
    public interface IUserBL:IBaseBL<UserDto, UserModel, User>
    {
    }
}
