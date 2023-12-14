using Minio;
using Minio.DataModel.Args;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Intrinsics.X86;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Lib
{
    public class MinioService : IMinioService
    {
        private readonly IMinioClient _minioClient;
        public MinioService(IMinioClient minioClient)
        {
            _minioClient = minioClient;
        }

        public async Task<bool> BucketExistAsync(string bucketName)
        {
            var args = new BucketExistsArgs()
                .WithBucket(bucketName);
            return await _minioClient.BucketExistsAsync(args).ConfigureAwait(false);
        }

        public async Task<Stream> GetFileAsync(string bucketName, string objectName)
        {

            var stream = new MemoryStream();
            var args = new GetObjectArgs()
               .WithBucket(bucketName)
               .WithObject(objectName)
               .WithCallbackStream((st) =>
               {
                   // Copy nội dung từ stream trả về từ MinIO vào stream của bạn
                   st.CopyTo(stream);
               });

            _ = await _minioClient.GetObjectAsync(args).ConfigureAwait(false);
            stream.Position = 0;
            return stream;
        }

        public async Task MakeBucketAsync(string bucketName)
        {
            await _minioClient.MakeBucketAsync(
              new MakeBucketArgs()
                  .WithBucket(bucketName)
                ).ConfigureAwait(false);
        }

        public async Task UploadFileAsync(string bucketName, string objectName, Stream fileStream, string contentType = "application/octet-stream")
        {
            fileStream.Seek(0, SeekOrigin.Begin);
            var args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);
            _ = await _minioClient.PutObjectAsync(args).ConfigureAwait(false);
        }
    }
}
