using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Projects;
using WorkFM.Common.Data.Workspaces;
using WorkFM.Common.Dto;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.DL.Repos.Projects
{
    public class ProjectDL : BaseDL<Project>, IProjectDL
    {
        public ProjectDL(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<PagingResponse> GetList(ParamQueryProject paramsQuery)
        {
            var cmd = @$"SELECT p.*,up.UserRole,up.IsFavorite 
                                FROM project p 
                                INNER JOIN user_project up  ON 
                                    p.Id = up.ProjectId   AND up.UserId = @UserId ";
            if(paramsQuery.Owner)
            {
                cmd += " AND p.UserId = up.UserId ";
            }
            cmd += @$"WHERE p.WorkspaceId = @WorkspaceId";

            var res = await _uow.Connection.QueryAsync<Project>(cmd, paramsQuery);

            return new PagingResponse
            {
                Data = res.ToList()
            };
                                   
        }

        public async Task<PagingResponse> GetProjectsInWorkspaceAsync(Dictionary<string, object> parameters)
        {
            var cmd = $@"
                SELECT * FROM project p 
                WHERE 
                 p.WorkspaceId = @WorkspaceId
                 and
                ( p.Type = 1 and ( p.Id In (SELECT up.ProjectId FROM user_project up WHERE up.UserId = @UserId )) 
                 OR
                 (p.Type <> 1 AND EXISTS  (SELECT uw.Id FROM user_workspace uw WHERE uw.UserId = @userId ) )
                 );
                SELECT count(1) FROM project p 
                WHERE 
                 p.WorkspaceId = @WorkspaceId
                 and
                ( p.Type = 1 and ( p.Id In (SELECT up.ProjectId FROM user_project up WHERE up.UserId = @UserId )) 
                 OR
                 (p.Type <> 1 AND EXISTS  (SELECT uw.Id FROM user_workspace uw WHERE uw.UserId = @userId ) )
                 );
                    ";
            var dynamicParams = new DynamicParameters(parameters);
            var multi = await _uow.Connection.QueryMultipleAsync(cmd, dynamicParams);
            var datas = multi.Read<Project>().ToList();
            var totalRecords = multi.ReadFirstOrDefault<int>();

            return new PagingResponse
            {
                Data = datas,
                TotalRecords = totalRecords
            };
        }
    }
}
