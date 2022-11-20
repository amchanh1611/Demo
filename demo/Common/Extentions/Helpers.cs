using Demo.AppSettings;
using Demo.DTO;
using MimeKit;
using System.Text;

namespace Demo.Common.Extentions
{
    public static class Helpers
    {
        public static string Base64UrlSafeEncode(this MimeMessage message)
        {
            byte[] inputByte = Encoding.UTF8.GetBytes(message.ToString());

            return Convert.ToBase64String(inputByte).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
        public static MimeMessage CreateEmailMessage(this Message message, string emailFrom)
        {
            MimeMessage emailMessage = new();
            emailMessage.From.Add(new MailboxAddress("Test ", emailFrom));
            emailMessage.To.Add(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }
    }
}
