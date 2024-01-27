using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Cards;
using WorkFM.DL.Repos.Bases;

namespace WorkFM.DL.Repos.Cards
{
    public interface ICardDL:IBaseDL<Card>
    {
        public Task<int> CreateCardAtatchmentAsync(CardAttachment cardAttachment);
        //public Task<int> DeleteCardAttachmentByAttachmentIdAsync();
    }
}
