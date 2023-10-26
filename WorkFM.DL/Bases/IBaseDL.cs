using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common;

namespace WorkFM.DL.Base
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
        public Task<TEntity> GetByIdAsync(Guid id);
        public Task<PagingResponse> GetAllAsync();
        public Task<dynamic> GetPagingAsync(Dictionary<string, object> parameters, List<string> listCoumns);
        public Task<int> CreateAsync(TEntity entity);
        public Task<int> CreateMultiAsync(List<TEntity> entities);
        public Task<int> UpdateAsync(TEntity entity);
        public Task<int> DeleteAsync(Guid id);


        //public Task<dynamic> QueryUsingCommandTextAsync( string sql, Dictionary<string, object> parameters);

        //public Task<int> ExecuteUsingCommandTextAsync(string sql, object parameters);

        //// ----------- using store --------------------
        //public Task<IEnumerable<TEntity>> QueryUsingStoreAsync(string storeName, Dictionary<string, object> parameters);
        //public Task<int> ExecuteUsingStoreAsync(string storeName, Dictionary<string, object> parameters);


    }
}
