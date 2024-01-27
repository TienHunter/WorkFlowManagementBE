using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Models.Users;
using WorkFM.DL.Repos.Bases;

namespace WorkFM.DL.Repos.Users
{

    public interface IUserDL : IBaseDL<User>
    {
        // lấy thông tin user theo username or email
        public Task<User> GetUserByUsername(string username);

        public Task<User> GetUserByEmail(string email);

        public Task<User> GetUserByUsernameOrEmail(string usernameOrEmail);
        public Task<int> UpdateImageUrlAsync(Guid id, string imageUrl);
    }
}
