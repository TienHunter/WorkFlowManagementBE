using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Jobs;
using WorkFM.Common.Dto;

namespace WorkFM.BL.Services.Jobs
{
    public interface IJobBL:IBaseBL<Job, Job>
    {
        public Task<ServiceResponse> CraeteAsync(JobCreateDto jobCreateDto);

        /// <summary>
        /// cập nhật trạng thái công viêc theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Task<ServiceResponse> UpdateStatusAsync(Guid id, bool status);

        /// <summary>
        /// cập nhật job khi di chuyển
        /// </summary>
        /// <param name="jobMoveDto"></param>
        /// <returns></returns>
        public Task<ServiceResponse> MoveAsync(JobMoveDto jobMoveDto);
    }
}
