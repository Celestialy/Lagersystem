using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Storage
{
    public class StorageSerivce : IStorageService
    {
        private readonly BlobServiceClient _client;

        public StorageSerivce(BlobServiceClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Validate file formats
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        private string GenerateFileName(string fileName) => Guid.NewGuid() + DateTime.Now.ToString("dd-MM-yyyy") + '.' + fileName.Split('.').Last();

        public async Task<Uri> UploadImageToStorage(IFormFile file)
        {
            if (!IsImage(file))
            {
                throw new ArgumentException("File is not a supported image type", nameof(file));
            }
            var blobContainer = _client.GetBlobContainerClient("images");
            var blobClient = blobContainer.GetBlobClient(GenerateFileName(file.FileName));
            await blobClient.UploadAsync(file.OpenReadStream());

            return blobClient.Uri;
        }
    }
}
