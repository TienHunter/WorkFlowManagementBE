using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Workspaces;
using WorkFM.Common.Dto;

namespace WorkFM.BL.Services.Workspaces
{
    public interface IWorkspaceBL:IBaseBL<Workspace,Workspace>
    {
        /// <summary>
        /// create workspace
        /// </summary>
        /// <param name="workspaceCreateDto"></param>
        /// <returns></returns>
        public Task<ServiceResponse> CreateWorkspaceAsync(WorkspaceCreateDto workspaceCreateDto);
        /// <summary>
        /// update workspace
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workspaceUpdateDto"></param>
        /// <returns></returns>
        public Task<ServiceResponse> UpdateWorkspaceAsync(Guid id,WorkspaceUpdateDto workspaceUpdateDto );

        /// <summary>
        /// get all workspace user join
        /// </summary>
        /// <returns></returns>
        public Task<ServiceResponse> GetAllAsync();

        /// <summary>
        /// get workspace by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ServiceResponse> GetByIdAsync(Guid id);
    }
}
