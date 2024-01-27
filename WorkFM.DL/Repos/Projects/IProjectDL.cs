using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Projects;
using WorkFM.Common.Data.Workspaces;
using WorkFM.Common.Dto;
using WorkFM.DL.Repos.Bases;

namespace WorkFM.DL.Repos.Projects
{
    public interface IProjectDL:IBaseDL<Project>
    {
        /// <summary>
        /// get projects in workspace 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<PagingResponse> GetProjectsInWorkspaceAsync(Dictionary<string, object> parameters);

        /// <summary>
        /// get list projects with query param
        /// </summary>
        /// <param name="paramsQuery"></param>
        /// <returns></returns>
        Task<PagingResponse> GetList(ParamQueryProject paramsQuery);

        Task<int> UpdateImageUrlAsync(Guid id, string imageUrl);

        Task<Guid> GetProjectIdByJobId(Guid id);
    }
}
