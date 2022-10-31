using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Helper
{
    public static class Helper
    {
        public static async Task<string> UploadFilesAsync(IFormFile formFile)
        {

            string relativePath = $"Avatar/{DateTime.UtcNow.Ticks + formFile.FileName}";

            string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            using (Stream stream = new FileStream(absolutePath, FileMode.Create))
            {
                 await formFile.CopyToAsync(stream);
            }

            return relativePath;
        }
    }
}
