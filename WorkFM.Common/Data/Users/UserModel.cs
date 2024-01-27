using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Data.Users
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProviderId { get; set; }
        public string Provider { get; set; }
        public string Fullname { get; set; }
        public string ImageUrl { get; set; }



    }
}
