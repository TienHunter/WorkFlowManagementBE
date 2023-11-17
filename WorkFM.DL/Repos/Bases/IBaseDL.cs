using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Dto;

namespace WorkFM.DL.Repos.Bases
{
    /// <summary>
    /// interface BaseDL
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// created by: vdtien (22/10/2023)
    public interface IBaseDL<TEntity>
    {
        // ------------ using command text -----------------------
        public Task<dynamic> QueryUsingCommandText(string sql, Dictionary<string, object> parameters);
        public Task<int> ExecuteUsingCommandText(string sql, Dictionary<string, object> parameters);
        /// <summary>
        /// lấy bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TEntity> GetByIdAsync(Guid id);
        public Task<PagingResponse> GetAllAsync();
        public Task<PagingResponse> GetPagingAsync(Dictionary<string, object> parameters, List<string> listCoumns);

        /// <summary>
        /// tạo bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> CreateAsync(TEntity entity);
        public Task<int> CreateMultiAsync(List<TEntity> entities);
        public Task<int> UpdateAsync(TEntity entity, string fields = null);
        public Task<int> DeleteAsync(Guid id);

        /// <summary>
        /// lấy bản ghi theo fields
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        //public Task<TEntity> GetByFields(string fields, Dictionary<string, object> parameters);


        /// <summary>
        /// lấy 1 bản ghi theo câu lệnh
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<TEntity> QuerySingleAsync(string cmd, Dictionary<string, object> parameters);
    }   
}
