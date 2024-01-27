using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Lib
{
    public interface IMinioService
    {
        public Task UploadFileAsync(string bucketName, string objectName, Stream fileStream, string contentType);
        public Task<Stream> GetFileAsync(string bucketName, string objectName);
        public Task<bool> BucketExistAsync(string bucketName);
        public Task MakeBucketAsync(string bucketName);
        public Task RemoveObjectAsync(string bucketName, string objectName);
    }
}
