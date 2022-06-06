using System;
using System.Net;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Oldsu.Web.Utils
{

    public static class EmailSender
    {
        private static readonly string Username =
            Environment.GetEnvironmentVariable("OLDSU_WEB_EMAIL_USERNAME") ?? string.Empty;

        private static readonly string Password =
            Environment.GetEnvironmentVariable("OLDSU_WEB_EMAIL_PASSWORD") ?? string.Empty;

        private static readonly string
            Host = Environment.GetEnvironmentVariable("OLDSU_WEB_EMAIL_HOST") ?? string.Empty;

        private static readonly int Port =
            int.Parse(Environment.GetEnvironmentVariable("OLDSU_WEB_EMAIL_PORT") ?? "465");

        public static async Task SendAsync(string target, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Oldsu", "noreply@ayyeve.xyz"));
            message.To.Add(new MailboxAddress(target, target));
            message.Subject = subject;
            message.Body = new TextPart("plain") {Text = body};

            using var client = new SmtpClient();
            
            await client.ConnectAsync(Host, Port);
            await client.AuthenticateAsync(Username, Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}