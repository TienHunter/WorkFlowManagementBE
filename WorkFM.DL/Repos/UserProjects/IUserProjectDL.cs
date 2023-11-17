using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.UserProjects;
using WorkFM.DL.Repos.Bases;

namespace WorkFM.DL.Repos.UserProjects
{
    public interface IUserProjectDL:IBaseDL<UserProject>
    {
        /// <summary>
        /// lấy ra quan hệ người dùng với project
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<UserProject> GetByProjectIdAndUserId(Dictionary<string, object> parameters);
    }
}
