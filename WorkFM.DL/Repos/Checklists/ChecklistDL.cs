using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Checklists;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.DL.Repos.Checklists
{
    public class ChecklistDL : BaseDL<Checklist>, IChecklistDL
    {
        public ChecklistDL(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
