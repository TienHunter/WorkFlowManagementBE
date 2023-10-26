using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Users;
using WorkFM.Common.Models.Users;
using WorkFM.DL.DatabaseService;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.BL.Services.Users
{
    public class UserBL : BaseBL<UserDto, UserModel, User>, IUserBL
    {
        public UserBL(IDatabaseService databaseService, IMapper mapper, IUnitOfWork uow) : base(databaseService, mapper, uow)
        {
        }
    }
}
