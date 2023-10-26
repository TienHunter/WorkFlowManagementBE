using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common;
using WorkFM.Common.Commands;
using WorkFM.Common.Enums;
using WorkFM.Common.Models;
using WorkFM.Common.Models.Users;
using WorkFM.Common.Utils;
using WorkFM.DL.Base;
using WorkFM.DL.DatabaseService;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.BL.Services.Bases
{
    public abstract class BaseBL<TDto, TEntity> : IBaseBL<TDto, TEntity>
    {
        #region Field

        protected readonly IBaseDL<TEntity> _baseDL;
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _uow;

        #endregion

        #region Constructor

        protected BaseBL(IBaseDL<TEntity> baseDL, IMapper mapper, IUnitOfWork uow)
        {
            _baseDL = baseDL;
            _mapper = mapper;
            _uow = uow;
        }
        #endregion
        #region Methods

        public Task<ServiceResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            throw new NotImplementedException ();
        }

        public async Task<ServiceResponse> GetByIdAsync(Guid id)
        {
            var entity = await _baseDL.GetByIdAsync(id);
            var model = _mapper.Map<TEntity>(entity);
            var dto = _mapper.Map<TDto>(model);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Code = 0,
                    Success = true,
                    Message = "Not found"
                };
            }
            return new ServiceResponse
            {
                Success = true,
                Code = 0,
                Message = "Oke",
                Data = dto
            };
        }

        public async Task<ServiceResponse> GetPagignAsync(PagingRequest pagingRequest)
        {
            if (pagingRequest == null || (pagingRequest.PageIndex == -1 && pagingRequest.PageSize == -1))
            {
                var resAll = await _baseDL.GetAllAsync();
                var dtos = _mapper.Map<List<TDto>>(resAll.Data);
                resAll.Data = dtos;
                return new ServiceResponse
                {
                    Success = true,
                    Code = 0,
                    Data = resAll
                };
            }
            pagingRequest.PageIndex = pagingRequest?.PageIndex ?? 0;
            pagingRequest.PageSize = pagingRequest?.PageSize??0;
            pagingRequest.KeySearch = pagingRequest?.KeySearch??"";
            pagingRequest.ColumnsFilter = pagingRequest?.ColumnsFilter ?? "";

             pagingRequest.ColumnsFilter = Helper.HandleSQLColumn(pagingRequest.ColumnsFilter);
            var listCoumns = pagingRequest.ColumnsFilter.Split(",").ToList();
            var parameters = new Dictionary<string, object>()
            {
                {"Limit" , pagingRequest.PageSize },
                {"Offset",(pagingRequest.PageIndex-1)*pagingRequest.PageSize },
                {"KeySearch",pagingRequest.KeySearch },

            };
            var res = await _baseDL.GetPagingAsync(parameters, listCoumns);

            // 
        }

        public Task<ServiceResponse> InsertAsync(TDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> UpdateAsync(TDto dto)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
