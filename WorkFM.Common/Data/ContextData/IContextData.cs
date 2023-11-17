using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Data.ContextData
{
    public interface IContextData
    {
        Guid UserId { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        public string Name { get; set; }
    }
}
