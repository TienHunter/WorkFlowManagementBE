using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Jobs;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.DL.Repos.Jobs
{
    public class JobDL : BaseDL<Job>, IJobDL
    {
        public JobDL(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
