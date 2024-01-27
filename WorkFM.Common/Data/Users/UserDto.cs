using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Bases;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Models.Users
{
    public class UserDto: BaseDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Provider { get; set; }
        public string Fullname { get; set; }
        public string ImageUrl { get; set; }
    }
}
