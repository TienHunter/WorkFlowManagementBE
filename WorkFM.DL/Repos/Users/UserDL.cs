using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Commands;
using WorkFM.Common.Models.Users;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.DL.Repos.Users
{
    public class UserDL : BaseDL<User>, IUserDL
    {
        public UserDL(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var sql = QueryCommand.GetUserByEmail();
            return await _uow.Connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var sql = QueryCommand.GetUserByUsername();
            return await _uow.Connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<User> GetUserByUsernameOrEmail(string usernameOrEmail)
        {
            var sql = QueryCommand.GetUserByUsernameOrEmail();
            return await _uow.Connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = usernameOrEmail });
        }

        public async Task<int> UpdateImageUrlAsync(Guid id, string imageUrl)
        {
            var cmd = $"UPDATE {_tableName} SET ImageUrl = @imageUrl WHERE Id = @id";
            return await _uow.Connection.ExecuteAsync(cmd, new {imageUrl, id}, transaction: _uow.Transaction);
        }
    }
}
