using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.ContextData;
using WorkFM.Common.Data.UserWorkspaces;
using WorkFM.Common.Data.Workspaces;
using WorkFM.Common.Dto;
using WorkFM.Common.Enums;
using WorkFM.Common.Exceptions;
using WorkFM.Common.Lib;
using WorkFM.DL.Repos.UserWorkspaces;
using WorkFM.DL.Repos.Workspaces;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.BL.Services.Workspaces
{
    public class WorkspaceBL : BaseBL<Workspace, Workspace>, IWorkspaceBL
    {
        private readonly IWorkspaceDL _workspaceDL;
        private readonly IUserWorkspaceDL _userWorkspaceDL;
        private readonly IDbLogger<WorkspaceBL> _logger;
        public WorkspaceBL(IServiceProvider serviceProvider, IWorkspaceDL workspaceDL) : base(serviceProvider, workspaceDL)
        {
            _workspaceDL = workspaceDL;
            _userWorkspaceDL = serviceProvider.GetService(typeof(IUserWorkspaceDL)) as IUserWorkspaceDL;
            _logger = serviceProvider.GetService(typeof(IDbLogger<WorkspaceBL>)) as IDbLogger<WorkspaceBL>;
        }

        /// <summary>
        /// tạo mới không gian làm việc
        /// </summary>
        /// <param name="workspaceCreateDto"></param>
        /// <returns></returns>
        public async Task<ServiceResponse> CreateWorkspaceAsync(WorkspaceCreateDto workspaceCreateDto)
        {
            var workspace = _mapper.Map<Workspace>(workspaceCreateDto);
            // bổ sung thêm các trường;
            workspace.UserId = _contextData.UserId;
            base.BeforeCreate(ref workspace);

            // user_workspace
            var userWorkspace = new UserWorkspace
            {
                UserId = _contextData.UserId,
                WorkspaceId = workspace.Id,
                UserRole = UserRole.Admin
            };
            base.BeforeCreate(ref userWorkspace);

            try
            {
                // open transaction
                await _uow.BeginTransactionAsync();

                await _workspaceDL.CreateAsync(workspace);
                await _userWorkspaceDL.CreateAsync(userWorkspace);

                await _uow.CommitAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
                await _uow.RollbackAsync();
                throw ex;
            }

            //await _workspaceDL.CreateAsync(workspace);
            return new ServiceResponse
            {
                Data = workspace
            };

        }

        /// <summary>
        /// get all workspace user join
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse> GetAllAsync()
        {
           var res = await _workspaceDL.GetAllAsync(_contextData.UserId);
            return new ServiceResponse
            {
                Data = res
            };
        }

        public async Task<ServiceResponse> GetByIdAsync(Guid id)
        {
            var res = await _workspaceDL.GetWorkspaceByIdAsync(id, _contextData.UserId) ?? throw new BaseException
            {
                StatusCode=HttpStatusCode.NotFound,
            };
            var resDto = _mapper.Map<WorkspaceDto>(res);
            return new ServiceResponse
            {
                Data = resDto
            };
        }

        /// <summary>
        /// chỉnh sửa không gian làm việc
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workspaceUpdateDto"></param>
        /// <returns></returns>
        public async Task<ServiceResponse> UpdateWorkspaceAsync(Guid id, WorkspaceUpdateDto workspaceUpdateDto)
        {
            // check exist
            workspaceUpdateDto.Id = id;
            var existWorkspace = await _workspaceDL.GetByIdAsync(id) ?? throw new BaseException
            {
                StatusCode = HttpStatusCode.NotFound,
                ErrorMessage = "Not found"
            };

            // check quyền của user có được sửa không
            var parameters = new Dictionary<string, object>()
            {
                {"@UserId",_contextData.UserId },
                {"@WorkspaceId",id }
            };
            var userWorkspace = await _userWorkspaceDL.GetByUserIdAndWorkspaceId(parameters);
            if (userWorkspace == null || userWorkspace.UserRole == UserRole.Member) throw new BaseException { StatusCode = HttpStatusCode.Unauthorized, ErrorMessage="Not authorized" };

            // pass update set value updateDto -> entity
            _mapper.Map<WorkspaceUpdateDto, Workspace>(workspaceUpdateDto, existWorkspace);
            base.BeforeUpdate<Workspace>(ref existWorkspace);

            // update
            if (await _workspaceDL.UpdateAsync(existWorkspace) == 0) throw new BaseException
            {
                ErrorMessage = "Cập nhật thất bại"
            };
            // map enitity -> dto

            return new ServiceResponse
            {
                Data = existWorkspace
            };
           
        }
    }
}
