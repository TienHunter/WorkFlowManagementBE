using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.UserProjects;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.DL.Repos.UserProjects
{
    public class UserProjectDL : BaseDL<UserProject>, IUserProjectDL
    {
        public UserProjectDL(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<UserProject> GetByProjectIdAndUserId(Dictionary<string, object> parameters)
        {
            var cmd = $"Select * from {_tableName} where ProjectId = @ProjectId and UserId = @UserId order by UpdatedAt DESC limit 1;";
            return await base.QuerySingleAsync(cmd, parameters);
        }
    }
}
