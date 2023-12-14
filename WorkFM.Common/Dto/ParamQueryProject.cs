using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Dto
{
    public class ParamQueryProject
    {
        [Required(ErrorMessage = "workspaceId isn't empty")]
        public Guid WorkspaceId { get; set; }
        public Guid UserId { get; set; }
        public bool Owner {  get; set; }
    }
}
