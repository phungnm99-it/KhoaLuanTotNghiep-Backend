using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.UploadImageUtils
{
    public interface IUploadImage
    {
        Task<object> UploadImage(IFormFile image, string imageName);
    }
}
