using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data;
using WorkFM.Common.Models.Users;

namespace WorkFM.BL.Services.Jwt
{
    public interface IJwtSerivce
    {
        TokenModel GenerateJwtToken(User user);
    }
}
