using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Users;
using WorkFM.Common.Models.Users;

namespace WorkFM.BL.Services.Users
{
    public class UserProfile:Profile
    {
        public UserProfile() { 
            CreateMap<UserDto,UserModel>().ReverseMap();
            CreateMap<User,UserModel>().ReverseMap();
        }
    }
}
