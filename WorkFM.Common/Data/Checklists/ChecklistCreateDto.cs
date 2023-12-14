using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Data.Checklists
{
    public class ChecklistCreateDto
    {
        public Guid CardId { get; set; }
        public string ChecklistName { get; set; }
        public double SortOrder { get; set; }
    }
}
