using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;

namespace WorkFM.Common.Data.Users
{
    public class UserRegister
    {
        [Required]
        public string Username { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",ErrorMessage ="Invalid email")]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? ProviderId { get; set; }
        public string? Provider { get; set; }
        [Required]
        public string Fullname { get; set; }

        public LoginType LoginType { get; set; }
    }
}
