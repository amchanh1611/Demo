using Microsoft.AspNetCore.Http;
using static Google.Apis.Auth.GoogleJsonWebSignature;

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

        //public static async Task<Payload> VerifyGoogleToken(string accesToken)
        //{
        //    //Settings used when validating a JSON Web Signature.
        //    ValidationSettings settings = new()
        //    {
        //        //GetSection : Gets a configuration sub-section with the specified key.
        //        Audience = new List<string>() { google.GetSection("clientId").Value }
        //    };
        //    // Validates a Google-issued Json Web Token (JWT)
        //    //Returns:
        //    //     The payload of the verified token.
        //    Payload payload = await ValidateAsync(accesToken.IdToken, settings);

        //    return payload;
        //}
    }
}