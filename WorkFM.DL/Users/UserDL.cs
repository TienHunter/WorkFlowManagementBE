using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Models.Users;
using WorkFM.DL.Base;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.DL.Users
{
    public class UserDL : BaseDL<User>, IUserDL
    {
        public UserDL(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
