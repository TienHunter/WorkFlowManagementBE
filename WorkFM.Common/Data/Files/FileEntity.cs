using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Data.Files
{
    [Table("attachment")]
    public class FileEntity:BaseEntity, IsHasInfoCreate, IsHasInfoUpdate
    {
        public string BucketName { get; set; }
        public string ObjectName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public double Size { get; set; }
        public AttachmentType Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
