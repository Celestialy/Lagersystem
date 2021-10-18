using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Storage
{
    public interface IStorageService
    {
        Task<Uri> UploadImageToStorage(IFormFile file);

    }
}
