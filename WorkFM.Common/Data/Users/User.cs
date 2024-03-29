﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Models.Users
{
    [Table("user")]
    public class User:BaseEntity, IsHasInfoCreate, IsHasInfoUpdate
    {
        //[Key]
        //public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProviderId { get; set; }
        public string Provider { get; set; }
        public string Fullname { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
