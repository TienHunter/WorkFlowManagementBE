using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Commands;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Data.Checklists;
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
            var cmd = @$"SELECT c.*,cl.*,j.* FROM card c
                                LEFT JOIN checklist cl ON cl.CardId = c.Id 
                                LEFT JOIN job j ON j.ChecklistId = cl.Id
                                WHERE c.Id = @id ORDER BY cl.SortOrder,j.SortOrder;";
            var cardDictionary = new Dictionary<Guid, Card>();
            var result = await _uow.Connection.QueryAsync<Card, Checklist, Job, Card>(cmd, (card, checklist, job) =>
            {
                if (!cardDictionary.TryGetValue(card.Id, out var cardEntry))
                {
                    cardEntry = card;
                    cardEntry.Checklists = new List<Checklist>();
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

                return cardEntry;

            }, splitOn: "Id,Id,Id", param: new { id });

            return result.Distinct().ToList().SingleOrDefault();
        }
    }
}
