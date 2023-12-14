using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Kanbans;
using WorkFM.DL.Repos.Bases;

namespace WorkFM.DL.Repos.Kanbans
{
    public interface IKanbanDL:IBaseDL<Kanban>
    {
        public Task<List<Kanban>> GetListByProjectIdAsync(Guid projectId);
    }
}
