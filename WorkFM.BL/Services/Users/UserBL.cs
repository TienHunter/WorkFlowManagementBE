using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.ContextData;
using WorkFM.Common.Data.Users;
using WorkFM.Common.Dto;
using WorkFM.Common.Enums;
using WorkFM.Common.Models.Users;
using WorkFM.DL.Repos.Users;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.BL.Services.Users
{
    public class UserBL : BaseBL<UserDto, User>, IUserBL
    {
        private readonly IUserDL _userDL;

        public UserBL(IServiceProvider serviceProvider, IUserDL userDL) : base(serviceProvider, userDL)
        {
            _userDL = userDL;
        }


        public async Task<ServiceResponse> CreateUser(UserRegister userRegister)
        {
            var serviceResponse = new ServiceResponse();

            // validate input dung attribute

            // ckeck exist user by username
           var userExist = await _userDL.GetUserByUsername(userRegister.Username);
            if(userExist != null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Code = ServiceResponseCode.UsernameTaken,
                    Message = "Username is already taken"
                };
            }

            // check exist user by email
             userExist = await _userDL.GetUserByEmail(userRegister.Email);
            if (userExist != null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Code = ServiceResponseCode.EmailTaken,
                    Message = "Email is already taken"
                };
                
            }

            // create user
            var user = _mapper.Map<User>(userRegister);
            base.BeforeCreate<User>(ref user);

            var res = await _userDL.CreateAsync(user);
            if(res == 0)
            {
              return new  ServiceResponse
                {
                    Success = false,
                    Code = ServiceResponseCode.Error,
                    Message = "Create user false"
                };
            }
            return new ServiceResponse
            {
                Success = true,
                Code = ServiceResponseCode.Success,
                Message = "Oke"
            };
        }

        //public override void BeforeCreate(ref User user)
        //{
        //    base.BeforeCreate(ref user);
        //    user.Id = Guid.NewGuid();
        //}

    }
}
