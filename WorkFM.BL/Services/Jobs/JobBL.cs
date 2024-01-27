using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Checklists;
using WorkFM.Common.Data.Jobs;
using WorkFM.Common.Dto;
using WorkFM.Common.Exceptions;
using WorkFM.Common.Lib;
using WorkFM.Common.Utils;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Repos.Checklists;
using WorkFM.DL.Repos.Jobs;
using WorkFM.DL.Repos.Projects;

namespace WorkFM.BL.Services.Jobs
{
    public class JobBL : BaseBL<Job, Job>, IJobBL
    {
        private readonly IJobDL _jobDL;
        private readonly IChecklistDL _checklistDL;
        private readonly IProjectDL _projectDL;
        private readonly IHubContext<ProjectHub> _hubContext;
        public JobBL(IServiceProvider serviceProvider, IJobDL jobDL, IHubContext<ProjectHub> hubContext) : base(serviceProvider, jobDL)
        {
            _jobDL = jobDL;
            _checklistDL = serviceProvider.GetService(typeof(IChecklistDL)) as IChecklistDL;
            _projectDL = serviceProvider.GetService(typeof(IProjectDL)) as IProjectDL;
            _hubContext = hubContext;
        }

        public async Task<ServiceResponse> CraeteAsync(JobCreateDto jobCreateDto)
        {
            var job = _mapper.Map<Job>(jobCreateDto);
            base.BeforeCreate<Job>(ref job);

            var res = await _jobDL.CreateAsync(job);
            if (res == 0)
            {
                throw new BaseException
                {
                    ErrorMessage = "Create job failure"
                };
            }
            return new ServiceResponse
            {
                Data = job,
            };


        }

        public async Task<ServiceResponse> MoveAsync(JobMoveDto jobMoveDto)
        {
            // check exit 
            var jobExist = await _jobDL.GetByIdAsync(jobMoveDto.Id) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found checklist"
            };
            // check permission

            // check card exist
            var checklistExist = await _checklistDL.GetByIdAsync(jobMoveDto.ChecklistId) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found card"
            };

            // check thẻ thuộc dự án nào có quyền truy cập không

            // check permission ...

            var fieldUpdate = "ChecklistId,SortOrder";
            // update cardExsit
            jobExist.ChecklistId = jobMoveDto.ChecklistId;
            jobExist.SortOrder = jobMoveDto.SortOrder;

            // update
            await _jobDL.UpdateAsync(jobExist, fieldUpdate);

            return new ServiceResponse();
        }

        public async Task<ServiceResponse> UpdateStatusAsync(Guid id, bool status)
        {
            // check exist
            var jobExist = await _jobDL.GetByIdAsync(id) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found job"
            };

            var fields = "IsFinished,FinishDate,UpdatedAt,UpdatedBy";
            base.BeforeUpdate<Job>(ref jobExist);
            jobExist.IsFinished = status;
            if (status)
            {
                jobExist.FinishDate = _systenService.GetNow();
            }
            else
            {
                jobExist.FinishDate = null;

            }
            await _jobDL.UpdateAsync(jobExist, fields);

            //var projectId = await _projectDL.GetProjectIdByJobId(id);
            //var endpoint = $"/job/project/{projectId}";
            //await _hubContext.Clients.Group(endpoint).SendAsync("SendJob", jobExist);
            // tạo ra 1 task để chạy
           // await TaskCreator.CreateTaskAsync(async () =>
           //{
           //    var projectId = await _projectDL.GetProjectIdByJobId(id);
           //    var endpoint = $"job/project/{projectId}";
           //    await _hubContext.Clients.Group(endpoint).SendAsync("SendJob", jobExist);
           //}, TimeSpan.FromDays(1));
            return new ServiceResponse();
        }
    }
}
