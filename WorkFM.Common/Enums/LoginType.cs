using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Enums
{
    public enum LoginType
    {
        LoginByUsername=1, // đăng nhập bằng tài khoản và mật khẩu
        LoginByEmail=2, // đăng nhập bằng oauth
    }
}
