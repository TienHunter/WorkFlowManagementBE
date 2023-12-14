using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Data.Cards
{
    [Table("card_attachment")]
    public class CardAttachment
    {
        public Guid CardId { get; set; }
        public Guid AttachmentId { get; set; }
    }
}
