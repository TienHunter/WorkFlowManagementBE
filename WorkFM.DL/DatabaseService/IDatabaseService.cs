using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.DL.DatabaseService
{
    /// <summary>
    /// interface database service
    /// </summary>
    public interface IDatabaseService
    {
        public Task<IEnumerable<T>> QueryMultiByCommandText<T>(string sql, Dictionary<string, object>? parameters);

        public Task<IEnumerable> QueryMultiByProc(string procName, Dictionary<string, object> parameters);

        public Task ExecuteByCommnandTextAsync(string sql, Dictionary<string, object> parameters);
    }
}
