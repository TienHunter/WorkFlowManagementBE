using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Commands;
using WorkFM.Common.Data.ContextData;
using WorkFM.Common.Dto;
using WorkFM.Common.Enums;
using WorkFM.Common.Lib;
using WorkFM.Common.Models.Base;
using WorkFM.Common.Models.Users;
using WorkFM.Common.Utils;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Service.UnitOfWork;
using static Dapper.SqlMapper;

namespace WorkFM.BL.Services.Bases
{
    public abstract class BaseBL<TDto, TEntity> : IBaseBL<TDto, TEntity>
    {
        #region Field

        protected readonly IServiceProvider _serviceProvider;
        protected readonly IBaseDL<TEntity> _baseDL;
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _uow;
        protected readonly IContextData _contextData;
        protected readonly ISystenService _systenService;
        protected readonly IDbLogger<BaseBL<TDto, TEntity>> _logger;
        #endregion

        #region Constructor

        protected BaseBL(IServiceProvider serviceProvider, IBaseDL<TEntity> baseDL)
        {
            _serviceProvider = serviceProvider;
            _baseDL = baseDL;
            _mapper = serviceProvider.GetService(typeof(IMapper)) as IMapper;
            _uow = serviceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            _contextData = serviceProvider.GetService(typeof(IContextData)) as IContextData;
            _systenService = serviceProvider.GetService(typeof(ISystenService)) as ISystenService;
            _logger = serviceProvider.GetService(typeof(IDbLogger<BaseBL<TDto, TEntity>>)) as IDbLogger<BaseBL<TDto, TEntity>>;
        }
        #endregion

        //#region Methods

        //public async Task<ServiceResponse> GetByIdAsync(Guid id)
        //{
        //    var entity = await _baseDL.GetByIdAsync(id);
        //    var dto = _mapper.Map<TDto>(entity);

        //    if (entity == null)
        //    {
        //        return new ServiceResponse
        //        {
        //            Code = 0,
        //            Success = true,
        //            Message = "Not found"
        //        };
        //    }
        //    return new ServiceResponse
        //    {
        //        Success = true,
        //        Code = 0,
        //        Message = "Oke",
        //        Data = dto
        //    };
        //}

        //public Task<ServiceResponse> DeleteAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<ServiceResponse> GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}



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
            pagingRequest.PageSize = pagingRequest?.PageSize ?? 0;
            pagingRequest.KeySearch = pagingRequest?.KeySearch ?? "";
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
            res.Data = _mapper.Map<List<TDto>>(res.Data);
            return new ServiceResponse
            {
                Success = true,
                Code = 0,
                Data = res
            };

            // 
        }

        //public async Task<ServiceResponse> CreateAsync(TDto dto)
        //{
        //    var serviceResponse = new ServiceResponse();

        //    // Validate
        //    if (!ValidateCreate(dto, ref serviceResponse))
        //    {
        //        return serviceResponse;
        //    }

        //    // BeforeCreate
        //    var entity = _mapper.Map<TEntity>(dto);
        //    BeforeCreate(ref entity);

        //    // DoCreate
        //    var res = await DoCreateAsync(entity);
        //    if (res == 0)
        //    {
        //        serviceResponse = new ServiceResponse
        //        {
        //            Success = false,
        //            Code = ServiceResponseCode.Error,
        //            Message = "Create false",
        //        };
        //    }


        //    // AfterCreate
        //    AfterCreate(ref dto, entity);

        //    serviceResponse = new ServiceResponse
        //    {
        //        Success = true,
        //        Code = ServiceResponseCode.Success,
        //        Message = "Oke",
        //        Data = dto
        //    };

        //    // Trả về ServiceResponse thành công
        //    return serviceResponse;
        //}

        //public Task<ServiceResponse> UpdateAsync(TDto dto)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual bool ValidateCreate(TDto dto, ref ServiceResponse serviceResponse)
        //{
        //    bool isValid = true;
        //    return isValid;
        //}

        public virtual void BeforeCreate<T>(ref T entity)
        {
            if (entity is BaseEntity)
            {
                var baseEntity = entity as BaseEntity;
                baseEntity.Id = _systenService.NewGuid();
            };
            if (entity is IsHasInfoCreate)
            {
                var hasCreateInfo = entity as IsHasInfoCreate;
                hasCreateInfo.CreatedAt = _systenService.GetUtcNow();
                hasCreateInfo.CreatedBy = _contextData.UserId.ToString();
            }
            if (entity is IsHasInfoUpdate)
            {
                var hasUpdateInfo = entity as IsHasInfoUpdate;
                hasUpdateInfo.UpdatedAt = _systenService.GetUtcNow();
                hasUpdateInfo.UpdatedBy = _contextData.UserId.ToString();
            }
        }

        public void BeforeUpdate<T>(ref T entity)
        {
            if (entity is IsHasInfoUpdate)
            {
                var hasUpdateInfo = entity as IsHasInfoUpdate;
                hasUpdateInfo.UpdatedAt = _systenService.GetUtcNow();
                hasUpdateInfo.UpdatedBy = _contextData.UserId.ToString();
            }
        }

        //public async Task<int> DoCreateAsync(TEntity entity)
        //{
        //    return await _baseDL.CreateAsync(entity);

        //}

        //public virtual void AfterCreate(ref TDto dto, TEntity entity)
        //{
        //    dto = _mapper.Map<TDto>(entity);
        //}


        //#endregion
    }
}
