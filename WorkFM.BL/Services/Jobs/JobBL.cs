using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Jobs;
using WorkFM.Common.Dto;
using WorkFM.Common.Exceptions;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Repos.Jobs;

namespace WorkFM.BL.Services.Jobs
{
    public class JobBL : BaseBL<Job, Job>, IJobBL
    {
        private readonly IJobDL _jobDL;
        public JobBL(IServiceProvider serviceProvider, IJobDL jobDL) : base(serviceProvider, jobDL)
        {
            _jobDL = jobDL;
        }

        public async Task<ServiceResponse> CraeteAsync(JobCreateDto jobCreateDto)
        {
            var job = _mapper.Map<Job>(jobCreateDto);
            base.BeforeCreate<Job>(ref job);

            var res = await _jobDL.CreateAsync(job);
            if(res == 0)
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
    }
}
