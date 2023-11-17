using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Workspaces;
using WorkFM.Common.Dto;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Service.UnitOfWork;
using static Dapper.SqlMapper;

namespace WorkFM.DL.Repos.Workspaces
{
    public class WorkspaceDL : BaseDL<Workspace>, IWorkspaceDL
    {
        public WorkspaceDL(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<PagingResponse> GetAllAsync(Guid userId)
        {
            var cmd = $@"
                SELECT w.* FROM workspace w INNER JOIN user_workspace uw WHERE w.Id = uw.WorkspaceId AND uw.UserId= @userId;
                SELECT COUNT(1) FROM workspace w INNER JOIN user_workspace uw WHERE w.Id = uw.WorkspaceId AND uw.UserId=@userId;
                ";
            var multi = await _uow.Connection.QueryMultipleAsync(cmd, new { userId });
            var datas = multi.Read<Workspace>().ToList();
            var totalRecords = multi.ReadFirstOrDefault<int>();

            return new PagingResponse
            {
                Data = datas,
                TotalRecords = totalRecords
            };

        }

        /// <summary>
        /// get workspace by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Workspace> GetWorkspaceByIdAsync(Guid id, Guid userId)
        {
            var cmd = $"SELECT * FROM workspace WHERE Id = @id AND EXISTS (SELECT * FROM user_workspace WHERE WorkspaceId = @id AND UserId= @userId)";

            return await _uow.Connection.QueryFirstOrDefaultAsync<Workspace>(cmd, new { id, userId });
        }
    }
}
