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
            if (paramsQuery.Owner)
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

        public async Task<Guid> GetProjectIdByJobId(Guid id)
        {
            var cmd = @$"SELECT
                                  p.Id
                                FROM project p
                                  INNER JOIN kanban k
                                    ON p.Id = k.ProjectId
                                  INNER JOIN card c
                                    ON k.Id = c.KanbanId
                                  INNER JOIN checklist c1
                                    ON c.Id = c1.CardId
                                  INNER JOIN job j
                                    ON c1.Id = j.ChecklistId
                                WHERE j.Id = @id;";
            var rs = await _uow.Connection.QueryFirstOrDefaultAsync<Guid>(cmd, new { id });
            return rs;
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

        public async Task<int> UpdateImageUrlAsync(Guid id, string imageUrl)
        {
            var cmd = $"UPDATE {_tableName} SET ImageUrl = @imageUrl WHERE Id = @id";
            return await _uow.Connection.ExecuteAsync(cmd, new { imageUrl, id }, transaction: _uow.Transaction);
        }
    }
}
