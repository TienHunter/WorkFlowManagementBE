using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common;
using WorkFM.Common.Commands;
using WorkFM.DL.Service.UnitOfWork;
using static Dapper.SqlMapper;

namespace WorkFM.DL.Base
{
    public abstract class BaseDL<TEntity> : IBaseDL<TEntity>
    {
        #region Fied

        protected readonly IUnitOfWork _uow;

        #endregion

        public BaseDL(IUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// convert tham số vào DynamicParameters
        /// </summary>
        /// <param name="parammetes"></param>
        /// <returns>dynamicParams</returns>
        /// created by: vdtien (25/10/2023)
        private static DynamicParameters ConvertParams(Dictionary<string, object> parammetes)
        {
            var dynamicParams = new DynamicParameters();
            foreach (var item in parammetes)
            {
                dynamicParams.Add($"@{item.Key}", item.Value);
            }
            return dynamicParams;
        }

        public async Task<int> CreateAsync(TEntity entity)
        {
            var sql = QueryCommand.Create<TEntity>();

            return await _uow.Connection.ExecuteAsync(sql, entity,transaction:_uow.Transaction);
        }

        public async Task<int> CreateMultiAsync(List<TEntity> entities)
        {
            var sql = QueryCommand.Create<TEntity>();
            return await _uow.Connection.ExecuteAsync(sql, entities, transaction:_uow.Transaction);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var sql = QueryCommand.Delete<TEntity>();
            return await _uow.Connection.ExecuteAsync(sql, new {Id = id}, transaction:_uow.Transaction);
        }

        /// <summary>
        /// thực hiện câu lệnh chung chưa xác định trước
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteUsingCommandText(string sql, Dictionary<string, object> parameters)
        {
            var dynamicParams = new DynamicParameters(parameters);
            return await _uow.Connection.ExecuteAsync(sql, dynamicParams, transaction: _uow.Transaction);
        }

        public async Task<PagingResponse> GetAllAsync()
        {
            var sql = QueryCommand.GetAll<TEntity>();
            var multi =  await _uow.Connection.QueryMultipleAsync(sql);
            return new PagingResponse
            {
                Data = multi.Read<dynamic>().ToList(),
                TotalRecords = multi.ReadFirstOrDefault<int>(),
                TotalRoots = multi.ReadFirstOrDefault<int>()
            };
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            var sql = QueryCommand.GetById<TEntity>();

            return await _uow.Connection.QueryFirstOrDefaultAsync<TEntity>(sql, new { Id = id });

        }

        public async Task<dynamic> GetPagingAsync(Dictionary<string, object> parameters,List<string> listCoumns)
        {
            var sql = QueryCommand.GetPaging<TEntity>(listCoumns);
            var dynamicParams = new DynamicParameters(parameters);

            var multi = await _uow.Connection.QueryMultipleAsync(sql);
            return new PagingResponse
            {
                Data = multi.Read<dynamic>().ToList(),
                TotalRecords = multi.ReadFirstOrDefault<int>(),
                TotalRoots = multi.ReadFirstOrDefault<int>()
            };
        }

        public Task<dynamic> QueryUsingCommandText(string sql, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
