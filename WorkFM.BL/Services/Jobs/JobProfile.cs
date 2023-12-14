using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Jobs;

namespace WorkFM.BL.Services.Jobs
{
    public class JobProfile:Profile
    {
        public JobProfile() {
            CreateMap<JobCreateDto, Job>();
        }
    }
}
