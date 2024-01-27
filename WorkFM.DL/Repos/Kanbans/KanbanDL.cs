using Dapper;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Data.Checklists;
using WorkFM.Common.Data.Files;
using WorkFM.Common.Data.Jobs;
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
            var cmd = @$"SELECT k.*,c.*,cl.*,j.*,a.* from kanban k 
                                LEFT JOIN card c on k.Id = c.KanbanId
                                LEFT JOIN checklist cl ON cl.CardId = c.Id 
                                LEFT JOIN job j ON j.ChecklistId = cl.Id
                                LEFT JOIN card_attachment ca ON ca.CardId = c.Id
                                LEFT JOIN attachment a ON a.Id = ca.AttachmentId
                            WHERE k.ProjectId = @projectId ORDER BY k.SortOrder,c.SortOrder,a.CreatedAt,cl.SortOrder,j.SortOrder";
            var kanbanDictionary = new Dictionary<Guid, Kanban>();

            var result = await _uow.Connection.QueryAsync<Kanban, Card, Checklist, Job, FileEntity, Kanban>(cmd, (kanban, card, checklist, job, attachment) =>
            {
                if (!kanbanDictionary.TryGetValue(kanban.Id, out var kanbanEntry))
                {
                    kanbanEntry = kanban;
                    kanbanEntry.Cards = new List<Card>();
                    kanbanDictionary.Add(kanbanEntry.Id, kanbanEntry);
                }

                if (card != null)
                {
                    if (!kanbanEntry.Cards.Any(c => c.Id == card.Id))
                    {
                        card.Checklists = new List<Checklist>();
                        card.Attachments = new List<FileEntity>();
                        kanbanEntry.Cards.Add(card);
                    }

                }
                if (checklist != null && card != null)
                {

                    var currentCard = kanbanEntry.Cards.FirstOrDefault(c => c.Id == card.Id);
                    if (currentCard != null && !currentCard.Checklists.Any(cl => cl.Id == checklist.Id))
                    {
                        checklist.Jobs = new List<Job>();
                        currentCard.Checklists.Add(checklist);
                    }
                }

                if (job != null && card != null && checklist != null)
                {
                    var currentCard = kanbanEntry.Cards.FirstOrDefault(c => c.Id == card.Id);
                    var currentChecklist = currentCard?.Checklists.FirstOrDefault(cl => cl.Id == checklist.Id);
                    if (currentChecklist != null && !currentChecklist.Jobs.Any(j => j.Id == job.Id))
                    {
                        currentChecklist.Jobs.Add(job);
                    }
                }
                if (attachment != null && card != null)
                {
                    var currentCard = kanbanEntry.Cards.FirstOrDefault(c => c.Id == card.Id);
                    if (currentCard != null && !currentCard.Attachments.Any(a => a.Id == attachment.Id))
                    {
                        currentCard.Attachments.Add(attachment);
                    }
                }


                return kanbanEntry;
            }, splitOn: "Id,Id,Id,Id,Id", param: new { projectId });

            return result.Distinct().ToList();
        }
    }
}
