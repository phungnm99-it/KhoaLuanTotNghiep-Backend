using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.UploadImageUtils
{
    public class UploadImageWithCloudinary : IUploadImage
    {
        public async Task<object> UploadImage(IFormFile image, string imageName, string folder)
        {
            Account account = new Account(
                "dobsh4rbw",
                "735983481477219",
                "Supej3JVT8AzpH_9AvMtMzzaYMY");
            Cloudinary cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()),
                PublicId = folder + imageName.Split(".")[0] + DateTime.Now.ToString("MMddyyhhmmss")
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult;
        }
    }
}
