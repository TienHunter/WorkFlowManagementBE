using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;

namespace WorkFM.Common.Data.Users
{
    public class UserLogin
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ProviderId { get; set; }
        public LoginType LoginType { get; set; }
    }
}
