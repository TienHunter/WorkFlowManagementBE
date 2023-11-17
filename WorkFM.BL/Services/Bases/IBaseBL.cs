using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Dto;

namespace WorkFM.BL.Services.Bases
{
    public interface IBaseBL<TDto, TEntity>
    {
        //public Task<ServiceResponse> GetByIdAsync(Guid id);

        //public Task<ServiceResponse> GetAllAsync();

        //public Task<ServiceResponse> CreateAsync(TDto dto);

        //public Task<ServiceResponse> UpdateAsync(TDto dto);

        //public Task<ServiceResponse> DeleteAsync(Guid id);

        public Task<ServiceResponse> GetPagignAsync(PagingRequest pagingRequest);

        //public bool ValidateCreate(TDto dto, ref ServiceResponse serviceResponse);

        public void BeforeCreate<T>(ref T entity);

        public void BeforeUpdate<T>(ref T entity);

        //public Task<int> DoCreateAsync(TEntity entity);

        //public void AfterCreate(ref TDto dto,TEntity entity );
    }
}
