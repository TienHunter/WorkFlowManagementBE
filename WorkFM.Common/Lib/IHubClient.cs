using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Jobs;

namespace WorkFM.Common.Lib
{
    public interface IHubClient
    {
        Task BoardcastJob(string endpoint, string name,Job job);

    }
}
