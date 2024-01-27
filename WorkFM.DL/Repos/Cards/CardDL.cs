using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Commands;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Data.Checklists;
using WorkFM.Common.Data.Files;
using WorkFM.Common.Data.Jobs;
using WorkFM.Common.Data.Kanbans;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.DL.Repos.Cards
{
    public class CardDL : BaseDL<Card>, ICardDL
    {
        public CardDL(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<int> CreateCardAtatchmentAsync(CardAttachment cardAttachment)
        {
            var cmd = QueryCommand.Create<CardAttachment>();
            return await _uow.Connection.ExecuteAsync(cmd, cardAttachment,transaction: _uow.Transaction);
        }

        public override async Task<Card> GetByIdAsync(Guid id)
        {
            var cmd = @$"SELECT c.*,cl.*,j.*,a.* FROM card c
                                LEFT JOIN checklist cl ON cl.CardId = c.Id 
                                LEFT JOIN job j ON j.ChecklistId = cl.Id
                                LEFT JOIN card_attachment ca ON ca.CardId = c.Id
                                LEFT JOIN attachment a ON a.Id = ca.AttachmentId
                                WHERE c.Id = @id ORDER BY a.CreatedAt,cl.SortOrder,j.SortOrder;";
            var cardDictionary = new Dictionary<Guid, Card>();
            var result = await _uow.Connection.QueryAsync<Card, Checklist, Job, FileEntity, Card>(cmd, (card, checklist, job,attachment) =>
            {
                if (!cardDictionary.TryGetValue(card.Id, out var cardEntry))
                {
                    cardEntry = card;
                    cardEntry.Checklists = new List<Checklist>();
                    cardEntry.Attachments = new List<FileEntity>();
                    cardDictionary.Add(cardEntry.Id, cardEntry);
                }

                if (checklist != null)
                {
                    if (!cardEntry.Checklists.Any(cl => cl.Id == checklist.Id))
                    {
                        checklist.Jobs = new List<Job>();
                        cardEntry.Checklists.Add(checklist);
                    }
                }

                if (job != null)
                {
                    var currentChecklist = cardEntry.Checklists.FirstOrDefault(cl => cl.Id == checklist.Id);
                    if (currentChecklist != null && !currentChecklist.Jobs.Any(j => j.Id == job.Id))
                    {
                        currentChecklist.Jobs.Add(job);
                    }
                }
                if(attachment != null)
                {
                    if (!cardEntry.Attachments.Any(a => a.Id == attachment.Id))
                    {
                        cardEntry.Attachments.Add(attachment);
                    }
                }

                return cardEntry;

            }, splitOn: "Id,Id,Id,Id", param: new { id });

            return result.Distinct().ToList().SingleOrDefault();
        }
    }
}
