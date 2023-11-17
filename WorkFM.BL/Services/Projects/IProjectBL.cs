using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Projects;
using WorkFM.Common.Dto;

namespace WorkFM.BL.Services.Projects
{
    public interface IProjectBL:IBaseBL<ProjectDto,Project>
    {
        /// <summary>
        /// create project
        /// </summary>
        /// <param name="projectCreateDto"></param>
        /// <returns></returns>
        Task<ServiceResponse> CreateAsync(ProjectCreateDto projectCreateDto);

        /// <summary>
        /// update project by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectUpdateDto"></param>
        /// <returns></returns>
        Task<ServiceResponse> UpdateAsync(Guid id, ProjectUpdateDto projectUpdateDto);

        /// <summary>
        /// get projects in workspace
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResponse> GetProjectsInWorkspaceAsync(Guid id);
    }
}
