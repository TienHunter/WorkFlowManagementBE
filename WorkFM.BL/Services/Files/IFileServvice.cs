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
        public Task<ServiceResponse> UploadAvatarAsync(IFormFile file);
        public Task<ServiceResponse> UploadImageWorkspaceAsync(IFormFile file, Guid id);
        public Task<ServiceResponse> UploadImageProjectAsync(IFormFile file, Guid id);
        public Task<ServiceResponse> UploadFileAsync(IFormFile file, Guid id);
        public Task<FileDto> GetFileAsync(Guid attachId);
        public Task<FileDto> GetFileByObjectNameAsync(string objectName);
        public Task RemoveFileAsync(Guid id);
    }
}
