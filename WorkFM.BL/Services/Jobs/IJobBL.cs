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
    }
}
