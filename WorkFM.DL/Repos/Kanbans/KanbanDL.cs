using Dapper;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Data.Kanbans;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.DL.Repos.Kanbans
{
    public class KanbanDL : BaseDL<Kanban>, IKanbanDL
    {
        public KanbanDL(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<List<Kanban>> GetListByProjectIdAsync(Guid projectId)
        {
            var cmd = @$"SELECT * from kanban k 
                                    LEFT JOIN card c on k.Id = c.KanbanId
                                WHERE k.ProjectId = @projectId ORDER BY k.SortOrder,c.SortOrder";
            var kanbanDictionary = new Dictionary<Guid, Kanban>();

            var result = await _uow.Connection.QueryAsync<Kanban, Card, Kanban>(cmd, (kanban, card) =>
            {
                if (!kanbanDictionary.TryGetValue(kanban.Id, out var kanbanEntry))
                {
                    kanbanEntry = kanban;
                    kanbanEntry.Cards = new List<Card>();
                    kanbanDictionary.Add(kanbanEntry.Id, kanbanEntry);
                }

                if (card != null)
                {
                    kanbanEntry.Cards.Add(card);
                }

                return kanbanEntry;
            }, splitOn: "Id", param: new { projectId });

            return result.Distinct().ToList();
        }
    }
}
