using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using WorkFM.BL.Services.Files;
using WorkFM.Common.Dto;
using WorkFM.Common.Enums;

namespace WorkFM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FilesController : ControllerBase
    {
        private readonly IFileServvice _fileService;
        
        public FilesController(IFileServvice fileService)
        {
            _fileService = fileService;
        }
        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            var res = await _fileService.UploadAvatarAsync(file);
            return Ok(res);
        }
        [HttpPost("upload-inage-workspace/{id}")]
        public async Task<IActionResult> UploadImageWorkspace(IFormFile file,[FromRoute] Guid id)
        {
            var res = await _fileService.UploadImageWorkspaceAsync(file,id);
            return Ok(res);
        }
        [HttpPost("upload-image-project/{id}")]
        public async Task<IActionResult> UploadImageProject(IFormFile file,[FromRoute] Guid id)
        {
            var res = await _fileService.UploadImageProjectAsync(file,id);
            return Ok(res);
        }

        [HttpPost("upload-attachment/{id}")]
        public async Task<IActionResult> UploadFile( IFormFile file, [FromRoute][Required] Guid id)
        {
            var res = await _fileService.UploadFileAsync(file, id);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile([FromRoute] Guid id)
        {
            var fileDto = await _fileService.GetFileAsync(id);


            // Trả về FileStreamResult với MemoryStream và loại MIME
            return new FileStreamResult(fileDto.Stream, fileDto.ContentType)
            {
                FileDownloadName = fileDto.FileName
            };
            //return File(fileDto.Data, "application/octet-stream", fileDto.FileName);

        }

        [HttpGet("ObjectName/{objectName}")]
        public async Task<IActionResult> GetFileByObjectName([FromRoute] string objectName)
        {
            var fileDto = await _fileService.GetFileByObjectNameAsync(objectName);
            return new FileStreamResult(fileDto.Stream, "application/octet-stream")
            {
                FileDownloadName = fileDto.FileName
            };
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFile([FromRoute] Guid id)
        {
             await _fileService.RemoveFileAsync(id);
            return Ok();
         

        }
    }
}
