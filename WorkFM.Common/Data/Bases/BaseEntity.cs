using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Models.Base
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
      


    }

    public interface IsHasInfoCreate
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }

    public interface IsHasInfoUpdate
    {
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
