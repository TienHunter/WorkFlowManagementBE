using Microsoft.AspNetCore.Http;
using Minio.DataModel.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Configs;
using WorkFM.Common.Constants;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Data.ContextData;
using WorkFM.Common.Data.Files;
using WorkFM.Common.Dto;
using WorkFM.Common.Enums;
using WorkFM.Common.Exceptions;
using WorkFM.Common.Lib;
using WorkFM.Common.Utils;
using WorkFM.DL.Repos.Attachments;
using WorkFM.DL.Repos.Cards;
using WorkFM.DL.Repos.Projects;
using WorkFM.DL.Repos.Users;
using WorkFM.DL.Repos.Workspaces;
using WorkFM.DL.Service.UnitOfWork;

namespace WorkFM.BL.Services.Files
{
    public class FileService : IFileServvice
    {
        private readonly IFileDL _attachmentDL;
        private readonly IMinioService _minioService;
        private readonly IUserDL _userDL;
        private readonly IWorkspaceDL _workspaceDL;
        private readonly IProjectDL _projectDL;
        private readonly ISystenService _systenService;
        private readonly IContextData _contextData;
        private readonly IUnitOfWork _uow;
        private readonly ICardDL _cardDL;
        private readonly MinioStoreConfig _minioStoreConfig;
        public FileService(IServiceProvider serviceProvider, MinioStoreConfig minioStoreConfig)
        {
            _attachmentDL = serviceProvider.GetService(typeof(IFileDL)) as IFileDL;
            _minioService = serviceProvider.GetService(typeof(IMinioService)) as IMinioService;
            _systenService = serviceProvider.GetService(typeof(ISystenService)) as ISystenService;
            _contextData = serviceProvider.GetService(typeof(IContextData)) as IContextData;
            _uow = serviceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            _cardDL = serviceProvider.GetService(typeof(ICardDL)) as ICardDL;
            _minioStoreConfig = minioStoreConfig;
            _userDL = serviceProvider.GetService(typeof(IUserDL)) as IUserDL;
            _workspaceDL = serviceProvider.GetService(typeof(IWorkspaceDL)) as IWorkspaceDL;
            _projectDL = serviceProvider.GetService(typeof(IProjectDL)) as IProjectDL;
        }

        public async Task<FileDto> GetFileAsync(Guid attachId)
        {
            // get info attach
            var attachment = await _attachmentDL.GetByIdAsync(attachId) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found info attach"
            };
            try
            {
                var stream = await _minioService.GetFileAsync(attachment.BucketName, attachment.ObjectName) ?? throw new BaseException
                {
                    ErrorMessage = "Not found file attach"
                };

                    return new FileDto
                    {
                        FileName = attachment.FileName,
                        Stream = stream,
                        ContentType = attachment.ContentType,
                    };


            }
            catch (Exception ex)
            {
                throw new BaseException
                {
                    ErrorMessage = $"Dowload file from server error {ex.Message}"
                };
            }

        }

        public Task<FileDto> GetFileByObjectNameAsync(string objectName)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveFileAsync(Guid id)
        {
            var attachment = await _attachmentDL.GetByIdAsync(id);
            if (attachment == null) return;
            await _attachmentDL.DeleteAsync(id);
            await _minioService.RemoveObjectAsync(attachment.BucketName, attachment.ObjectName);
        }


        public async Task<ServiceResponse> UploadAvatarAsync(IFormFile file)
        {
            // check file anh
            if (file.Length > 0)
            {// Lấy đuôi của file
                string fileExtension = Path.GetExtension(file.FileName);
                var bucketName = BucketName.Avatar;
                var objectName = _systenService.NewGuid().ToString() + fileExtension;
                var contentType = file.ContentType;
                long length = file.Length;

                using (var strean = new MemoryStream())
                {
                    file.CopyTo(strean);

                    try
                    {

                        // Make a bucket on the server, if not already present.
                        var found = await _minioService.BucketExistAsync(bucketName);
                        if (!found)
                        {
                            await _minioService.MakeBucketAsync(bucketName);
                        }
                        await _minioService.UploadFileAsync(bucketName, objectName, strean, contentType);
                    }
                    catch (Exception ex)
                    {
                        // logger 
                        throw new BaseException
                        {
                            ErrorMessage = "Storage file to minio failure"
                        };
                    }
                }
                // update imageUrl
                var imageUrl = $"{_minioStoreConfig.Url}/{bucketName}/{objectName}";
                await _userDL.UpdateImageUrlAsync(_contextData.UserId, imageUrl);

                // url image
                return new ServiceResponse
                {
                    Data = imageUrl
                };
            }
            else
            {
                throw new BaseException
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = "Invalid input file"
                };
            }

        }

        /// <summary>
        /// upload file
        /// tạm thời chỉ upload file đính kèm của thẻ
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="BaseException"></exception>
        public async Task<ServiceResponse> UploadFileAsync(IFormFile file, Guid id)
        {
            if (file.Length > 0)
            {// Lấy đuôi của file
                string fileExtension = Path.GetExtension(file.FileName);
                var bucketName = "attachment";
                var objectName = _systenService.NewGuid().ToString() + fileExtension;
                var contentType = file.ContentType;
                long length = file.Length;
                using (var strean = new MemoryStream())
                {
                    file.CopyTo(strean);

                    try
                    {
                        // Make a bucket on the server, if not already present.
                        var found = await _minioService.BucketExistAsync(bucketName);
                        if (!found)
                        {
                            await _minioService.MakeBucketAsync(bucketName);
                        }
                        await _minioService.UploadFileAsync(bucketName, objectName, strean, contentType);
                    }
                    catch (Exception ex)
                    {
                        // logger 
                        throw new BaseException
                        {
                            ErrorMessage = "Storage file to minio failure"
                        };
                    }
                }
                // tạo ra đối tượng File để lưu vào db
                var fileEntity = new FileEntity
                {
                    Id = _systenService.NewGuid(),
                    BucketName = bucketName,
                    ObjectName = objectName,
                    FileName = file.FileName,
                    ContentType = contentType,
                    Size = length,
                    Type = AttachmentType.Attach,
                    CreatedAt = _systenService.GetNow(),
                    CreatedBy = _contextData.UserId.ToString(),
                    UpdatedAt = _systenService.GetNow(),
                    UpdatedBy = _contextData.UserId.ToString(),

                };
                var cardAttachment = new CardAttachment
                {
                    CardId = id,
                    AttachmentId = fileEntity.Id
                };
                try
                {
                    await _uow.BeginTransactionAsync();
                    await _attachmentDL.CreateAsync(fileEntity);
                    await _cardDL.CreateCardAtatchmentAsync(cardAttachment);
                    await _uow.CommitAsync();
                }
                catch (Exception ex)
                {
                    await _uow.RollbackAsync();
                    throw new BaseException
                    {
                        ErrorMessage = "Insert attact failure"
                    };
                }


                return new ServiceResponse
                {
                    Data = new { Bucketname = bucketName, ObjectName = objectName }
                };
            }
            else
            {
                throw new BaseException
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = "Invalid input file"
                };
            }


        }

        public async Task<ServiceResponse> UploadImageProjectAsync(IFormFile file, Guid id)
        {
            // check file anh
            if (file.Length > 0)
            {// Lấy đuôi của file
                string fileExtension = Path.GetExtension(file.FileName);
                var bucketName = BucketName.Avatar;
                var objectName = _systenService.NewGuid().ToString() + fileExtension;
                var contentType = file.ContentType;
                long length = file.Length;

                using (var strean = new MemoryStream())
                {
                    file.CopyTo(strean);

                    try
                    {

                        // Make a bucket on the server, if not already present.
                        var found = await _minioService.BucketExistAsync(bucketName);
                        if (!found)
                        {
                            await _minioService.MakeBucketAsync(bucketName);
                        }
                        await _minioService.UploadFileAsync(bucketName, objectName, strean, contentType);
                    }
                    catch (Exception ex)
                    {
                        // logger 
                        throw new BaseException
                        {
                            ErrorMessage = "Storage file to minio failure"
                        };
                    }
                }
                // update imageUrl
                var imageUrl = $"{_minioStoreConfig.Url}/{bucketName}/{objectName}";
                await _workspaceDL.UpdateImageUrlAsync(id, imageUrl);
                // url image
                return new ServiceResponse
                {
                    Data = imageUrl
                };
            }
            else
            {
                throw new BaseException
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = "Invalid input file"
                };
            }

        }

        public async Task<ServiceResponse> UploadImageWorkspaceAsync(IFormFile file, Guid id)
        {
            // check file anh
            if (file.Length > 0)
            {// Lấy đuôi của file
                string fileExtension = Path.GetExtension(file.FileName);
                var bucketName = BucketName.Avatar;
                var objectName = _systenService.NewGuid().ToString() + fileExtension;
                var contentType = file.ContentType;
                long length = file.Length;

                using (var strean = new MemoryStream())
                {
                    file.CopyTo(strean);

                    try
                    {

                        // Make a bucket on the server, if not already present.
                        var found = await _minioService.BucketExistAsync(bucketName);
                        if (!found)
                        {
                            await _minioService.MakeBucketAsync(bucketName);
                        }
                        await _minioService.UploadFileAsync(bucketName, objectName, strean, contentType);
                    }
                    catch (Exception ex)
                    {
                        // logger 
                        throw new BaseException
                        {
                            ErrorMessage = "Storage file to minio failure"
                        };
                    }
                }
                // update imageUrl
                var imageUrl = $"{_minioStoreConfig.Url}/{bucketName}/{objectName}";
                await _workspaceDL.UpdateImageUrlAsync(id, imageUrl);
                // url image
                return new ServiceResponse
                {
                    Data = imageUrl
                };
            }
            else
            {
                throw new BaseException
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = "Invalid input file"
                };
            }

        }
    }
}
