using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Utils
{
    /// <summary>
    /// các methods hệ thống
    /// </summary>
    /// created by: vdtien (3/11/2023)
    public interface ISystenService
    {
        /// <summary>
        /// lấy thời gian theo máy cục bộ
        /// </summary>
        /// <returns></returns>
        public DateTime GetNow();

        /// <summary>
        /// lấy thời gian hiện tại theo utc 
        /// </summary>
        /// <returns></returns>
        public DateTime GetUtcNow();

        public Guid NewGuid();
    }
}
