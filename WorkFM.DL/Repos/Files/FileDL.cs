using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Files;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.DL.Repos.Attachments
{
    public class FileDL : BaseDL<FileEntity>, IFileDL
    {
        public FileDL(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
