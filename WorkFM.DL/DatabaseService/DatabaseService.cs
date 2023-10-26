using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.DL.DatabaseService
{
    public class DatabaseService : IDatabaseService
    {
        protected readonly IUnitOfWork _uow;

        public DatabaseService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task ExecuteByCommnandTextAsync(string sql, Dictionary<string, object>? parameters)
        {
            // Tạo đối tượng DynamicParameters và thêm các tham số từ Dictionary
            var dynamicParameters = new DynamicParameters();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }

            }

            await _uow.Connection.ExecuteAsync(sql, dynamicParameters, transaction
                : _uow.Transaction);
        }

        public async Task<IEnumerable<T>> QueryMultiByCommandText<T>(string sql, Dictionary<string, object> parameters)
        {
            // Tạo đối tượng DynamicParameters và thêm các tham số từ Dictionary
            var dynamicParameters = new DynamicParameters();
            if(parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }

            }

            var result = await _uow.Connection.QueryAsync<T>(sql, dynamicParameters,transaction
                :_uow.Transaction);
            return result;

        }

        public Task<IEnumerable> QueryMultiByProc(string procName, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
