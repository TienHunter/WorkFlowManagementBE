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

        [HttpPost("upload-avatar/{type}/{id}")]
        public async Task<IActionResult> UploadAvatar(IFormFile file, [FromRoute][Required] AttachmentType type, [FromRoute][Required] Guid id)
        {
            var res = await _fileService.UploadAvatarAsync(file,type,id);
            return Ok(res);
        }

        [HttpPost("upload-attachment/{type}/{id}")]
        public async Task<IActionResult> UploadFile( IFormFile file, [FromRoute][Required] AttachmentType type, [FromRoute][Required] Guid id)
        {
            var res = await _fileService.UploadFileAsync(file, type, id);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile([FromRoute] Guid id)
        {
            var fileDto = await _fileService.GetFileAsync(id);

            return File(fileDto.Stream, "application/octet-stream", fileDto.ObjectName);

        }
    }
}
