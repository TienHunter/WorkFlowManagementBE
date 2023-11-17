using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.UserWorkspaces;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.DL.Repos.UserWorkspaces
{
    public class UserWorkspaceDL : BaseDL<UserWorkspace>, IUserWorkspaceDL
    {
        public UserWorkspaceDL(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<UserWorkspace> GetByUserIdAndWorkspaceId(Dictionary<string, object> parameters)
        {
            var cmd = $"Select * from {_tableName} where UserId=@UserId and WorkspaceId=@WorkspaceId order by CreatedAt DESC limit 1;";
            var dynamicParams = new DynamicParameters(parameters) ;
            return await _uow.Connection.QueryFirstOrDefaultAsync<UserWorkspace>(cmd, dynamicParams);
        }
    }
}
