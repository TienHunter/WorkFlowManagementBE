using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.UserWorkspaces;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Repos.UserWorkspaces;

namespace WorkFM.BL.Services.UserWorkspaces
{
    public class UserWorkspaceBL : BaseBL<UserWorkspace, UserWorkspace>, IUserWorkspaceBL
    {
        public UserWorkspaceBL(IServiceProvider serviceProvider, IUserWorkspaceDL userWorkspaceDL) : base(serviceProvider, userWorkspaceDL)
        {
        }
    }
}
