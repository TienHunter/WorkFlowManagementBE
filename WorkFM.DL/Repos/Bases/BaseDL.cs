using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WorkFM.Common.Commands;
using WorkFM.Common.Dto;
using WorkFM.Common.Exceptions;
using WorkFM.Common.Utils;
using WorkFM.DL.Service.UnitOfWork;
using static Dapper.SqlMapper;

namespace WorkFM.DL.Repos.Bases
{
    public abstract class BaseDL<TEntity> : IBaseDL<TEntity>
    {
        #region Fied

        protected readonly IUnitOfWork _uow;
        protected string _tableName;
        #endregion

        protected BaseDL(IUnitOfWork uow)
        {
            _uow = uow;
            _tableName = GetTableName();
        }

        /// <summary>
        /// convert tham số vào DynamicParameters
        /// </summary>
        /// <param name="parammetes"></param>
        /// <returns>dynamicParams</returns>
        /// created by: vdtien (25/10/2023)
        protected DynamicParameters ConvertParams(Dictionary<string, object> parammetes)
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

            return await _uow.Connection.ExecuteAsync(sql, entity, transaction: _uow.Transaction);
        }

        public async Task<int> CreateMultiAsync(List<TEntity> entities)
        {
            var sql = QueryCommand.Create<TEntity>();
            return await _uow.Connection.ExecuteAsync(sql, entities, transaction: _uow.Transaction);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var sql = QueryCommand.Delete<TEntity>();
            return await _uow.Connection.ExecuteAsync(sql, new { Id = id }, transaction: _uow.Transaction);
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
            var multi = await _uow.Connection.QueryMultipleAsync(sql);
            var datas = multi.Read<TEntity>().ToList();
            var totalRecords = 0;
            var totalRoots = 0;
            if (!multi.IsConsumed)
            {
                totalRecords = multi.ReadFirstOrDefault<int>();
            }
            if (!multi.IsConsumed)
            {
                totalRoots = multi.ReadFirstOrDefault<int>();

            }
            return new PagingResponse
            {
                Data = datas,
                TotalRecords = totalRecords,
                TotalRoots = totalRoots
            };
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            var sql = QueryCommand.GetById<TEntity>();

            return await _uow.Connection.QueryFirstOrDefaultAsync<TEntity>(sql, new { Id = id });

        }

        public async Task<PagingResponse> GetPagingAsync(Dictionary<string, object> parameters, List<string> listCoumns)
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

        /// <summary>
        /// update entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        /// <exception cref="BaseException"></exception>
        public async Task<int> UpdateAsync(TEntity entity, string fields = null)
        {
            var cmd = "";
            // update full fields
            if (string.IsNullOrEmpty(fields))
            {
                // build cmd
                cmd = QueryCommand.Update<TEntity>();
            }
            else
            {
                //  lay la property name cua entity
                var listPropertiesName = typeof(TEntity)
            .GetProperties()
            .Where(prop => !Attribute.IsDefined(prop, typeof(KeyAttribute)) && prop.Name != "Id")
            .Select(prop => prop.Name)
            .ToList();
                var pKey = typeof(TEntity).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)) || p.Name == "Id");

                if(pKey == null)
                {

                    throw new BaseException
                    {
                        ErrorMessage="Not found primary key"
                    };
                }
                // map field in fields voi property enitty
                var updateFields = new List<string>(){ };
                
                var listfields = fields.Split(',').ToList();
                foreach (var field in listfields)
                {
                    if(listPropertiesName.Contains(field))
                    {
                        updateFields.Add(field);
                    }
                }
                // build cmd
                if(updateFields.Count() > 0)
                cmd = $"Update {_tableName} SET {string.Join(" , ", updateFields.Select(f=> $"{f} = @{f}"))} Where {pKey}=@{pKey} ;";

            }

            return await _uow.Connection.ExecuteAsync(cmd, entity, transaction: _uow.Transaction);

        }


        /// <summary>
        /// lấy table name
        /// </summary>
        /// <returns></returns>
        protected static string GetTableName()
        {
            return Helper.GetTableName(typeof(TEntity));
        }

        public async Task<TEntity> QuerySingleAsync(string cmd, Dictionary<string, object> parameters)
        {
            var dynamicParams = new DynamicParameters(parameters);
            return await _uow.Connection.QueryFirstOrDefaultAsync<TEntity>(cmd, dynamicParams);
        }
    }
}
