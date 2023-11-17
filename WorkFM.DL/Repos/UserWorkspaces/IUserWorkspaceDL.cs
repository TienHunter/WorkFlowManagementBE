using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.UserWorkspaces;
using WorkFM.DL.Repos.Bases;

namespace WorkFM.DL.Repos.UserWorkspaces
{
    public interface IUserWorkspaceDL:IBaseDL<UserWorkspace>
    {
        /// <summary>
        /// get user_workspace by userId and workspaceId
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<UserWorkspace> GetByUserIdAndWorkspaceId(Dictionary<string, object> parameters);
    }
}
