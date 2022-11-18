using MimeKit;
using System.Text;

namespace Demo.Helpers
{
    public static class Helpers
    {
        public static string Base64UrlSafeEncode(this MimeMessage message)
        {
            byte[] inputByte = Encoding.UTF8.GetBytes(message.ToString());

            return Convert.ToBase64String(inputByte).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}
