using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common;
using WorkFM.Common.Models;

namespace WorkFM.BL.Services.Bases
{
    public interface IBaseBL<TDto, TEntity>
    {
        public Task<ServiceResponse> GetByIdAsync(Guid id);

        public Task<ServiceResponse> GetAllAsync();

        public Task<ServiceResponse> InsertAsync(TDto dto);

        public Task<ServiceResponse> UpdateAsync(TDto dto);

        public Task<ServiceResponse> DeleteAsync(Guid id);

        public Task<ServiceResponse> GetPagignAsync(PagingRequest pagingRequest);
    }
}
