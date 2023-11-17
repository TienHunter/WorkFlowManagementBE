using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Workspaces;
using WorkFM.Common.Dto;
using WorkFM.DL.Repos.Bases;

namespace WorkFM.DL.Repos.Workspaces
{
    public interface IWorkspaceDL : IBaseDL<Workspace>
    {
        /// <summary>
        /// get all workspace user has joined
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<PagingResponse> GetAllAsync(Guid userId);

        /// <summary>
        /// get workspace by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Workspace> GetWorkspaceByIdAsync(Guid id, Guid userId);

    }
}
