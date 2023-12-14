using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Dto;
using WorkFM.Common.Enums;

namespace WorkFM.BL.Services.Files
{
    public interface IFileServvice
    {
        public Task<ServiceResponse> UploadAvatarAsync(IFormFile file, AttachmentType type, Guid id);
        public Task<ServiceResponse> UploadFileAsync(IFormFile file, AttachmentType type, Guid id);
        public Task<FileDto> GetFileAsync(Guid attachId);
    }
}
